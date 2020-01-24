using System;

namespace CreateUseCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese una opción:");
            Console.WriteLine("1 - PHP");
            Console.WriteLine("2 - TypeScript");
            string option = Console.ReadLine();

            ICreateUseCase creator;

            switch (option)
            {
                case "1":
                    creator = new CreatePHPUseCase("/home/cristian/Documentos/Varios/testsphp");
                    break;
                case "2":
                    creator = new CreateTypeScriptUseCase("/home/cristian/Documentos/Varios/tests");
                    break;
                default:
                    Console.WriteLine("la opción no es valida, abortando");
                    return;
            }

            Console.Write("Ingrese un nombre de caso de uso: ");
            string name = Console.ReadLine();

            creator.Execute(name);

            Console.WriteLine("Hecho!");
        }
    }
}
