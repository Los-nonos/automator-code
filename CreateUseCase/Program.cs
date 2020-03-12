using System;
using CreateArchitecture;

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

            if (!verificator.VerificateExistArchitecture())
            {
                do
                {
                    Console.Write("La carpeta está vacía, ¿desea crear la arquitectura base? (y/n): ");
                    string option = Console.ReadLine();
                    if (option.ToLower() == "y")
                    {
                        var creatorArchitecture = new CreateBaseArchitecture(path);
                        creatorArchitecture.execute(TypesArchitectures.HEXAGONAL);
                        break;
                    }
                    else if (option.ToLower() == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ingrese una opcion valida");
                    }
                } while (true);
            }

            HandleTypescript(path);

            Console.WriteLine("Hecho!");
        }

        private static void HandleTypescript(string path)
        {
            Console.Write("Ingrese la categoria del caso de uso: ");
            string category = Console.ReadLine();
            if (!verificator.Verificate(category))
            {
                Console.Write("La categoria no existe, ¿desea crear el CRUD? (y/n): ");
                string _option = Console.ReadLine();
                switch (_option)
                {
                    case "y":
                        creator = new CreateCRUDTypescrypt(path);
                        Console.Write("Ingrese las variables a tener en cuenta: ");
                        string data = Console.ReadLine();
                        creator.Execute(category, data, category);
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
