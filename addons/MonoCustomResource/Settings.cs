using Godot;
using Godot.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonoCustomResourceRegistry
{
	public static class Settings
	{
		public enum ResourceSearchType
		{
			Recursive = 0,
			Namespace = 1,
		}

		public static string ClassPrefix => GetSettings(nameof(ClassPrefix)).ToString();
		public static ResourceSearchType SearchType => GetResourceSearchType();
		public static ReadOnlyCollection<string> ResourceScriptDirectories => GetResourceScriptDirectories();

		public static void Init()
		{
			string classPrefixTitle = SettingPath(nameof(ClassPrefix));
			string searchTypeTitle = SettingPath(nameof(SearchType));
			string resourceScriptDirTitle = SettingPath(nameof(ResourceScriptDirectories));

			SetupClassPrefixSetting(classPrefixTitle);
			SetupSearchTypeSetting(searchTypeTitle);
			SetupResourceScriptDirectoriesSetting(resourceScriptDirTitle);

			AddSettingPropertyInfo(classPrefixTitle, Variant.Type.String);
			AddSettingPropertyInfo(searchTypeTitle, Variant.Type.Int, PropertyHint.Enum, "Recursive,Namespace");
			AddSettingPropertyInfo(resourceScriptDirTitle, Variant.Type.PackedStringArray);
		}

		private static ResourceSearchType GetResourceSearchType()
		{
			var setting = ProjectSettings.GetSetting($"{nameof(MonoCustomResourceRegistry)}/{nameof(SearchType)}");
			return (ResourceSearchType) setting.AsInt32();
		}

		private static ReadOnlyCollection<string> GetResourceScriptDirectories() {
			var setting = ProjectSettings.GetSetting($"{nameof(MonoCustomResourceRegistry)}/{nameof(ResourceScriptDirectories)}");
			return new ReadOnlyCollection<string>(setting.AsStringArray().Cast<string>().ToList());
		}

		private static object GetSettings(string title)
		{
			return ProjectSettings.GetSetting($"{nameof(MonoCustomResourceRegistry)}/{title}");
		}

		private static void SetupClassPrefixSetting(string title)
		{
			if (!ProjectSettings.HasSetting(title))
				ProjectSettings.SetSetting(title, "");
		}

		private static void SetupSearchTypeSetting(string title)
		{
			if (!ProjectSettings.HasSetting(title))
				ProjectSettings.SetSetting(title, (int) ResourceSearchType.Recursive);
		}

		private static void SetupResourceScriptDirectoriesSetting(string title)
		{
			if (!ProjectSettings.HasSetting(title))
				ProjectSettings.SetSetting(title, new Array<string> (new string[]{ "res://"}));
		}

		private static void AddSettingPropertyInfo(string title, Variant.Type type, PropertyHint hint = PropertyHint.None, string hintString = "")
		{
			var info = new Godot.Collections.Dictionary
			{
				{"name", title},
				{"type", (long) type},
				{"hint", (long) hint},
				{"hint_string", hintString},
			};
			ProjectSettings.AddPropertyInfo(info);
		}

		private static string SettingPath(string title) => $"{nameof(MonoCustomResourceRegistry)}/{title}";
	}
}