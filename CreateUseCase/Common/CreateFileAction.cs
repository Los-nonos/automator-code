using System;
using System.IO;

namespace CreateUseCase
{
    class CreateFile
    {
        private string name_use_case;
        private string base_path;
        public CreateFile(string name_use_case, string base_path)
        {
            this.name_use_case = name_use_case;

            if(!Directory.Exists(base_path)){
                Directory.CreateDirectory(base_path);
            }
            this.base_path = base_path;
        }

        public void VerificateFileOrCreate(string path, string nameFile)
        {
            Console.WriteLine(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path_file = Path.Combine(path, nameFile);

            var a = File.Create(path_file);
            a.Dispose();
        }

        public string CreateName(string typeFile, string extension)
        {
            return string.Format("{0}{1}.{2}", this.name_use_case, typeFile, extension);
        }

        public string CreateName(string typeFile)
        {
            return string.Format("{0}{1}", this.name_use_case, typeFile);
        }

        public string CreateName(string nameUseCase, string typeFile, string extension)
        {
            return string.Format("{0}{1}.{2}", nameUseCase, typeFile, extension);
        }

        public void LoadFile(string path, string nameFile, string[] content)
        {   
            string _path = Path.Combine(path, nameFile);
            File.WriteAllLines(_path, content);
        }

        public string CombinePath(string path, string category)
        {
            return this.base_path + path + category;
        }
    }
}