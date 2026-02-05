using System;
using System.Net.Http.Headers;
using System.Reflection;


namespace lab11
{
	static class Reflector
	{
		public static string AssemblyName(string className)
		{
			Type type = Type.GetType(className);
			Assembly assembly = type.Assembly;
			return assembly.GetName().Name;
		}
		
		public static bool IsHaveConstructor(string className)
		{
			Type type = Type.GetType(className);
			if (type == null) return false;

			ConstructorInfo[] ctors = type.GetConstructors();
			return ctors.Any(c => c.GetParameters().Length > 0);
		}
		
		public static IEnumerable<string> PublicMethods(string className)
		{
			Type type = Type.GetType(className);
			MethodInfo[] methods = type.GetMethods();
			IEnumerable<string> a = methods.Select(a => a.Name);
			return a;
		}
		
		public static IEnumerable<string> PublicPropertiesAndFields(string className)
		{
			Type type = Type.GetType(className);
			FieldInfo[] fields = type.GetFields();
			PropertyInfo[] props = type.GetProperties();
			List<string> a = new List<string>();
			foreach (PropertyInfo p in props)
			{
				a.Add(p.Name);
			}

			foreach (FieldInfo f in fields)
			{
				a.Add(f.Name);
			}
			IEnumerable<string> b = a;
			return b;
		}

		public static IEnumerable<string> RealizedInterfaces(string className)
		{
			Type type = Type.GetType(className);
			Type[] interfaces = type.GetInterfaces();
			IEnumerable<string> b = interfaces.Select(i => i.FullName);
			return b;
		}
		
		public static string[] MethodsName(string className, string[] methodParameters)
		{
			Type type = Type.GetType(className);
			MethodInfo[] methods = type.GetMethods();
			List<string> a = new List<string>();
			foreach (MethodInfo m in methods)
			{
				ParameterInfo[] parameters = m.GetParameters();
				string[] parameterNames = parameters.Select(p => p.Name).ToArray();
				if (parameterNames == methodParameters)
					a.Add(m.Name);
			}
			return a.ToArray();
		}

		public static T Create<T>(Type type, object[] args)
		{
			return (T)Activator.CreateInstance(type, args);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Reflector.AssemblyName("lab11.Word"));
			Console.WriteLine("------------------------------------------");
			
			Console.WriteLine(Reflector.IsHaveConstructor("lab11.Conficker"));
			Console.WriteLine("------------------------------------------");
			
			foreach (var name in Reflector.PublicMethods("lab11.Conficker"))
			{
				Console.WriteLine(name);
			}
			Console.WriteLine("------------------------------------------");
			
			foreach (var name in Reflector.PublicPropertiesAndFields("lab11.Conficker"))
			{
				Console.WriteLine(name);
			}
			Console.WriteLine("------------------------------------------");
			
			foreach (var name in Reflector.RealizedInterfaces("lab11.Conficker"))
			{
				Console.WriteLine(name);
			}
			Console.WriteLine("------------------------------------------");
			
			foreach (var name in Reflector.MethodsName("lab11.Conficker", []))
			{
				Console.WriteLine(name);
			}
			Console.WriteLine("------------------------------------------");
			
			// g
			Random random = new Random();
			Type type;
			object obj;
			MethodInfo method;
			ParameterInfo[] param;
			object[] parameters;
			if (random.Next(2) == 0)
			{
				string[] lines = File.ReadAllLines("D:\\university\\GitHub\\C#\\11_lab\\lab11\\lab11\\file.txt");
				type = Type.GetType(lines[0]);
				obj = Activator.CreateInstance(type);
				method = type.GetMethod(lines[1]);
				string[] rawValues = lines[2].Split(' ')
					.Select(s => s.Trim()).ToArray();
				
				param = method.GetParameters();
				parameters = new object[param.Length];
				for (int i = 0; i < param.Length; i++)
				{
					Type targetType = param[i].ParameterType;
					string value = rawValues[i];

					parameters[i] = Convert.ChangeType(value, targetType);
				}
			}
			else
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				Type[] types = asm.GetTypes();
				type = types[random.Next(0, types.Length)];
				obj = Activator.CreateInstance(type);
				MethodInfo[] methods = type.GetMethods();
				method = methods[random.Next(0, methods.Length)];
				
				param = method.GetParameters();
				parameters = new object[param.Length];
				for (int i = 0; i < param.Length; i++)
				{
					Type targetType = param[i].ParameterType;
					parameters[i] = Convert.ChangeType(random.NextDouble(), targetType);
				}
			}
			
			var result = method.Invoke(obj, parameters);
			Console.WriteLine(result);
			
			// 2
			Reflector.Create<Conficker>(Type.GetType("lab11.Conficker"), []);
		}
	}
};