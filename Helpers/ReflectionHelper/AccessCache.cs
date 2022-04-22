using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace RFRocketLibrary.Helpers.ReflectionHelper
{
    internal class AccessCache
	{
		internal enum MemberType
		{
			Any,
			Static,
			Instance
		}

		private const BindingFlags BasicFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
		private static readonly Dictionary<MemberType, BindingFlags> DeclaredOnlyBindingFlags = new()
		{
			{ MemberType.Any, BasicFlags | BindingFlags.Instance | BindingFlags.Static },
			{ MemberType.Instance, BasicFlags | BindingFlags.Instance },
			{ MemberType.Static, BasicFlags | BindingFlags.Static }
		};

		private readonly Dictionary<Type?, Dictionary<string, FieldInfo?>> _declaredFields = new();
		private readonly Dictionary<Type?, Dictionary<string, PropertyInfo?>> _declaredProperties = new();
		private readonly Dictionary<Type?, Dictionary<string, Dictionary<int, MethodBase?>>> _declaredMethods = new();

		private readonly Dictionary<Type?, Dictionary<string, FieldInfo>> _inheritedFields = new();
		private readonly Dictionary<Type?, Dictionary<string, PropertyInfo>> _inheritedProperties = new();
		private readonly Dictionary<Type?, Dictionary<string, Dictionary<int, MethodBase>>> _inheritedMethods = new();

		private static T? Get<T>(Dictionary<Type?, Dictionary<string, T>> dict, Type? type, string name, Func<T> fetcher)
		{
			lock (dict)
			{
				if (dict.TryGetValue(type, out var valuesByName) is false)
				{
					valuesByName = new Dictionary<string, T>();
					dict[type] = valuesByName;
				}
				if (valuesByName.TryGetValue(name, out var value) is false)
				{
					value = fetcher();
					valuesByName[name] = value;
				}
				return value;
			}
		}

		private static T? Get<T>(Dictionary<Type?, Dictionary<string, Dictionary<int, T>>> dict, Type? type, string name, IEnumerable<Type> arguments, Func<T> fetcher)
		{
			lock (dict)
			{
				if (dict.TryGetValue(type, out var valuesByName) is false)
				{
					valuesByName = new Dictionary<string, Dictionary<int, T>>();
					dict[type] = valuesByName;
				}
				if (valuesByName.TryGetValue(name, out var valuesByArgument) is false)
				{
					valuesByArgument = new Dictionary<int, T>();
					valuesByName[name] = valuesByArgument;
				}
				var argumentsHash = AccessTools.CombinedHashCode(arguments);
				if (valuesByArgument.TryGetValue(argumentsHash, out var value) is false)
				{
					value = fetcher();
					valuesByArgument[argumentsHash] = value;
				}
				return value;
			}
		}

		internal FieldInfo? GetFieldInfo(Type? type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false)
		{
			var value = Get(_declaredFields, type, name, () => type?.GetField(name, DeclaredOnlyBindingFlags[memberType]));
			if (value is null && declaredOnly is false)
				value = Get(_inheritedFields, type, name, () => AccessTools.FindIncludingBaseTypes(type, t => t.GetField(name, AccessTools.all)));
			return value;
		}

		internal PropertyInfo? GetPropertyInfo(Type? type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false)
		{
			var value = Get(_declaredProperties, type, name, () => type?.GetProperty(name, DeclaredOnlyBindingFlags[memberType]));
			if (value is null && declaredOnly is false)
				value = Get(_inheritedProperties, type, name, () => AccessTools.FindIncludingBaseTypes(type, t => t.GetProperty(name, AccessTools.all)));
			return value;
		}

		internal MethodBase? GetMethodInfo(Type? type, string name, Type[] arguments, MemberType memberType = MemberType.Any, bool declaredOnly = false)
		{
			var value = Get(_declaredMethods, type, name, arguments, () => type?.GetMethod(name, DeclaredOnlyBindingFlags[memberType], null, arguments, null));
			if (value is null && declaredOnly is false)
				value = Get(_inheritedMethods, type, name, arguments, () => AccessTools.Method(type, name, arguments));
			return value;
		}
	}
}