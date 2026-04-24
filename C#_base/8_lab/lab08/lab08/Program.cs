
namespace  lab08
{
	delegate void Dedede(int a);

	class User
	{
		public event Dedede transport;
		public event Dedede compress;

		public void Transport(int a)
		{
			transport?.Invoke(a);
		}

		public void Compress(int a)
		{
			compress?.Invoke(a);
		}

		public void Tr1(int a)
		{
			Console.WriteLine($"Перемещаем на {a}");
		}

		public void Tr2(int a)
		{
			Console.WriteLine("БлаблаблаTr2Tr2");
		}

		public void Com1(int a)
		{
			Console.WriteLine($"Сжимаем на {a}");
		}

		public void Com2(int a)
		{
			Console.WriteLine("БлаблаблаCom2Com2");
		}

		public void All(int a)
		{
			Console.WriteLine($"Было что-то вызвано с параметром a = {a}\n");
		}
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			User user = new User();
			user.transport += user.Tr1;
			user.transport += user.Tr2;
			user.compress += user.Com1;
			user.compress += user.Com2;
			user.transport += user.All;
			user.compress += user.All;
			
			user.Compress(5);
			user.Transport(10);	
		}
	}
};




