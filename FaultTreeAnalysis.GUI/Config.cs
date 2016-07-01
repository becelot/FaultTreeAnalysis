using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FaultTreeAnalysis.GUI.Annotations;
using FaultTreeAnalysis.GUI.Util;

namespace FaultTreeAnalysis.GUI
{
	public class Config : INotifyPropertyChanged
	{
		private static Config config;

		public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
													+ @"\FaultTreeAnalysis";

		public static void Save() => XmlManager<Config>.Save(Instance.ConfigPath, Instance);


		public string ConfigPath => Instance.ConfigDir + "config.xml";

		public string ConfigDir => Instance.SaveConfigInAppData == false ? string.Empty : AppDataPath + "\\";

		[DefaultValue(true)]
		public bool? SaveConfigInAppData;

		private double errorTolerance = 1e-16;
		private double samplingRate = 0.5d;
		private double timeSpan = 40.0d;

	    private bool showWarningWhenRemoval = true;

	    [DefaultValue(40.0d)]
		public double TimeSpan
		{
			get { return this.timeSpan; }
			set
			{
				if (value.Equals(this.timeSpan)) return;
			    this.timeSpan = value;
			    this.OnPropertyChanged();
			}
		}

		[DefaultValue(0.5d)]
		public double SamplingRate
		{
			get { return this.samplingRate; }
			set
			{
				if (value.Equals(this.samplingRate)) return;
			    this.samplingRate = value;
			    this.OnPropertyChanged();
			}
		}

		[DefaultValue(1e-16)]
		public double ErrorTolerance
		{
			get { return this.errorTolerance; }
			set
			{
				if (value.Equals(this.errorTolerance)) return;
			    this.errorTolerance = value;
			    this.OnPropertyChanged();
			}
		}

	    [DefaultValue(true)]
	    public bool ShowWarningWhenRemoval
	    {
	        get
	        {
	            return this.showWarningWhenRemoval;
	        }
	        set
	        {
	            if (value.Equals(this.showWarningWhenRemoval)) return;
	            this.showWarningWhenRemoval = value;
                this.OnPropertyChanged();
	        }
	    }

	    public static Config Instance
		{
			get
			{
				if (config != null)
					return config;
				config = new Config();
				config.ResetAll();
				return config;
			}
		}


		public static void SaveBackup(bool deleteOriginal = false)
		{
			var configPath = Instance.ConfigPath;

			if (!File.Exists(configPath))
				return;

			File.Copy(configPath, configPath + DateTime.Now.ToFileTime());

			if (deleteOriginal)
				File.Delete(configPath);
		}

		public static void Load()
		{
			var foundConfig = false;
			Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
			try
			{
				if (File.Exists("config.xml"))
				{
					config = XmlManager<Config>.Load("config.xml");
					foundConfig = true;
				}
				else if (File.Exists(AppDataPath + @"\config.xml"))
				{
					config = XmlManager<Config>.Load(AppDataPath + @"\config.xml");
					foundConfig = true;
				}
				else if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
					//save locally if appdata doesn't exist (when e.g. not on C)
					Instance.SaveConfigInAppData = false;
			}
			catch (Exception e)
			{
				MessageBox.Show(
								e.Message + "\n\n" + e.InnerException + "\n\n If you don't know how to fix this, please delete "
								+ Instance.ConfigPath, "Error loading config.xml");
				Application.Current.Shutdown();
			}

			if (!foundConfig)
			{
				if (Instance.ConfigDir != string.Empty)
					Directory.CreateDirectory(Instance.ConfigDir);
				Save();
			}
			else if (Instance.SaveConfigInAppData != null)
			{
				if (Instance.SaveConfigInAppData.Value) //check if config needs to be moved
				{
					if (File.Exists("config.xml"))
					{
						Directory.CreateDirectory(Instance.ConfigDir);
						SaveBackup(true); //backup in case the file already exists
						File.Move("config.xml", Instance.ConfigPath);
					}
				}
				else if (File.Exists(AppDataPath + @"\config.xml"))
				{
					SaveBackup(true); //backup in case the file already exists
					File.Move(AppDataPath + @"\config.xml", Instance.ConfigPath);
				}
			}
		}

		public void ResetAll()
		{
			foreach (var field in this.GetType().GetFields())
			{
				var attr = (DefaultValueAttribute)field.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault();
				if (attr != null)
					field.SetValue(this, attr.Value);
			}
		}

		public void Reset(string name)
		{
			var proper = this.GetType().GetFields().First(x => x.Name == name);
			var attr = (DefaultValueAttribute)proper.GetCustomAttributes(typeof(DefaultValueAttribute), false).First();
			proper.SetValue(this, attr.Value);
		}

		[AttributeUsage(AttributeTargets.All, Inherited = false)]
		private sealed class DefaultValueAttribute : Attribute
		{
			// This is a positional argument
			public DefaultValueAttribute(object value)
			{
			    this.Value = value;
			}

			public object Value { get; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
		    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			Save();
		}
	}
}
