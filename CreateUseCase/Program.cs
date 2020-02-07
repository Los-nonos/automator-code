using System;

namespace CreateUseCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Ingrese la ruta de acceso donde se encuentra el repositorio: ");
            string path = Console.ReadLine();

            var verificator = new VerificateCategory(path);
            Console.WriteLine("Ingrese una opción:");
            Console.WriteLine("1 - PHP");
            Console.WriteLine("2 - TypeScript");
            string option = Console.ReadLine();

            ICreateUseCase creator;

            switch (option)
            {
                case "1":
                    creator = new CreatePHPUseCase(path);
                    break;
                case "2":
                    creator = new CreateTypeScriptUseCase(path);
                    break;
                default:
                    Console.WriteLine("la opción no es valida, abortando");
                    return;
            }

            Console.Write("Ingrese la categoria del caso de uso: ");
            string category = Console.ReadLine();

            if (!verificator.Verificate(category))
            {
                Console.Write("La categoria no existe, ¿desea crear el CRUD?: (y/n)");
                string _option = Console.ReadLine();
                switch (_option)
                {
                    case "y":
                        creator = new CreateCRUDTypescrypt("/home/cristian/Documentos/Varios/tests");
                        creator.Execute(category, null, category);
                        break;
                    case "n":
                        Console.Write("Ingrese un nombre de caso de uso: ");
                        string name = Console.ReadLine();
                        Console.Write("Ingrese las variables que desea tener en cuenta: ");
                        string info = Console.ReadLine();
                        creator.Execute(name, info, category);
                        break;
                    default:
                        Console.WriteLine("La opción no es valida, ingrese una");
                        break;
                }
            }

            Console.WriteLine("Hecho!");
        }
    }
}
