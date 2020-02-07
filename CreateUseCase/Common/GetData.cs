using System.Collections.Generic;
using System.IO;

namespace CreateUseCase
{
    class GetData
    {
        public GetData()
        {

        }

        public List<DataDTO> ClearData(string data)
        {
            var array = data.Split(' ', ',', '.');
            var streamContent = File.ReadAllLines("../CreateUseCase/Data/command.ts.data");

            List<DataDTO> info = new List<DataDTO>();
            foreach (var item in streamContent)
            {
                foreach (var a in array)
                {
                    if (item.Split('|')[0] == a)
                    {
                        var _array = item.Split('|');
                        info.Add(new DataDTO(_array[0], _array[1], _array[2], _array[3], _array[4]));
                    }
                }
            }

            if (info.Count != 0)
            {
                return info;
            }
            else
            {
                throw new System.Exception("no se pudo machear la información ingresada");
            }
        }


    }
}