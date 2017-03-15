using System;

namespace MARTIN_TEST
{
	class MainClass
	{
		class Foo
		{
			public int A { get; set; }
			public string B { get; set; }
		}


		public static void Main(string[] args)
		{
			TestClass tc = new TestClass();

			var fieldsName = string.Empty;

			foreach (var prop in tc.GetType().GetProperties())
			{
				fieldsName += "[" + prop.Name + "]";
				//if (fn != fn.Last())
				//{
				//	fieldsName += ",";
				//}
				Console.WriteLine(fieldsName);

			}


			Foo foo = new Foo { A = 1, B = "abc" };
			foreach (var prop in foo.GetType().GetProperties())
			{
				Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(foo, null));
			}

		}
	}
}
