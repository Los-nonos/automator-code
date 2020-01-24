using System.IO;

class CreateArchitecture
{
    private string path;

    public CreateArchitecture()
    {
        this.path = "/home/cristian/Documentos/tests";
        if (!Directory.Exists(this.path))
        {
            Directory.CreateDirectory(this.path);
        }
    }

    private void Create()
    {
        var directories = Directory.EnumerateDirectories(this.path);

        if (directories == null)
        {
            Directory.CreateDirectory(this.path + "/src");
            this.path = this.path + "/src";
            Directory.CreateDirectory(this.path + "Infraestructure");
        }
    }
}