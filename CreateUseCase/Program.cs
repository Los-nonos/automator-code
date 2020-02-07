using System;

namespace CreateUseCase
{
    class Program
    {
        static VerificateCategory verificator;
        static ICreateUseCase creator;
        static void Main(string[] args)
        {
            Console.Write("Ingrese la ruta de acceso donde se encuentra el repositorio: ");
            string path = Console.ReadLine();

            verificator = new VerificateCategory(path);
            Console.WriteLine("Ingrese una opción:");
            Console.WriteLine("1 - PHP");
            Console.WriteLine("2 - TypeScript");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    creator = new CreatePHPUseCase(path);
                    break;
                case "2":
                    HandleTypescript(path);
                    break;
                default:
                    Console.WriteLine("la opción no es valida, abortando");
                    return;
            }

            Console.WriteLine("Hecho!");
        }

        private static void HandleTypescript(string path)
        {
            Console.Write("Ingrese la categoria del caso de uso: ");
            string category = Console.ReadLine();
            Console.WriteLine(verificator.Verificate(category));
            if (!verificator.Verificate(category))
            {
                Console.Write("La categoria no existe, ¿desea crear el CRUD?: (y/n)");
                string _option = Console.ReadLine();
                switch (_option)
                {
                    case "y":
                        creator = new CreateCRUDTypescrypt(path);
                        creator.Execute(category, null, category);
                        return;
                    case "n":
                        break;
                    default:
                        Console.WriteLine("La opción no es valida, abortando");
                        return;
                }
            }
            
            creator = new CreateTypeScriptUseCase(path);
            Console.Write("Ingrese un nombre de caso de uso: ");
            string name = Console.ReadLine();
            Console.Write("Ingrese las variables que desea tener en cuenta: ");
            string info = Console.ReadLine();
            creator.Execute(name, info, category);
        }
    }
}
