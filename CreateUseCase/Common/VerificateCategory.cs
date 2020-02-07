using System.IO;

namespace CreateUseCase{

    class VerificateCategory{

        private string base_path;
        public VerificateCategory(string base_path){
            System.Console.WriteLine(base_path);
            this.base_path = base_path;
        }
        private string CombinePath(string path)
        {
            return this.base_path + path;
        }

        public bool Verificate(string category){
            string path = CombinePath(string.Format("/src/API/Http/Actions/{0}", category));
            System.Console.WriteLine(path);
            return Directory.Exists(path);
        }
    }
}