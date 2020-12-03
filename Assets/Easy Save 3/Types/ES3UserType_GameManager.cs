using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("TargetFrameRate", "MaximumLives", "CurrentLives", "GameOverScene", "Points")]
	public class ES3UserType_GameManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameManager() : base(typeof(MoreMountains.CorgiEngine.GameManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (MoreMountains.CorgiEngine.GameManager)obj;
			
			writer.WriteProperty("TargetFrameRate", instance.TargetFrameRate, ES3Type_int.Instance);
			writer.WriteProperty("MaximumLives", instance.MaximumLives, ES3Type_int.Instance);
			writer.WriteProperty("CurrentLives", instance.CurrentLives, ES3Type_int.Instance);
			writer.WriteProperty("GameOverScene", instance.GameOverScene, ES3Type_string.Instance);
			writer.WritePrivateProperty("Points", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (MoreMountains.CorgiEngine.GameManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "TargetFrameRate":
						instance.TargetFrameRate = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "MaximumLives":
						instance.MaximumLives = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "CurrentLives":
						instance.CurrentLives = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "GameOverScene":
						instance.GameOverScene = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Points":
					reader.SetPrivateProperty("Points", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_GameManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameManagerArray() : base(typeof(MoreMountains.CorgiEngine.GameManager[]), ES3UserType_GameManager.Instance)
		{
			Instance = this;
		}
	}
}