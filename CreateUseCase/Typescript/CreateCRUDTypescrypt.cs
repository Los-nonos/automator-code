
using System;
using System.Collections.Generic;

namespace CreateUseCase
{
    class CreateCRUDTypescrypt : ICreateUseCase
    {
        private string name_use_case;
        private string category;
        private string data;

        List<DataDTO> dataClear;
        private string base_path;
        private CreateFile createFile;
        private GetData getData;

        public CreateCRUDTypescrypt(string base_path)
        {
            this.base_path = base_path;
        }

        public void Execute(string name, string data, string category)
        {
            this.name_use_case = name;
            this.data = data;
            this.category = category;

            this.getData = new GetData();
            this.createFile = new CreateFile(name, base_path);
            this.dataClear = this.getData.ClearData(data);
            var generatorCode = new GeneratorCommonFiles(this.createFile, this.getData, name, category, this.dataClear);

            string[] array = new string[] { "Store", "Update", "Destroy", "Index", "Show" };
            foreach (var item in array)
            {
                var _data = string.Format("{0}{1}", item, name);
                generatorCode.CreateAction(_data);
                generatorCode.CreateAdapter(_data);
                generatorCode.CreateCommand(_data, data);
                generatorCode.CreateHandler(_data);
                generatorCode.CreatePresenter(_data);
            }
            this.CreateRouter(category);
            this.CreateEntity(category);
            this.CreateRepositoryInterface(category);
            this.CreateRepository(category);
            this.CreateSchema(category);
        }

        private void CreateEntity(string category)
        {
            string path = this.createFile.CombinePath(Paths.EntityPath, string.Empty);
            string fileName = this.createFile.CreateName("ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[] {
                "import { PrimaryGeneratedColumn, Column, Entity } from 'typeorm';",
                "",
                "@Entity()",
                string.Format("class {0}", category),
                "{",
                MatchInfo.GetPropsEntityTs(this.dataClear),
                "}",
                "",
                string.Format("export default {0}", category)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateRepositoryInterface(string category)
        {
            string path = this.createFile.CombinePath(Paths.RepositoryInterfacePath);
            string fileName = "I" + this.createFile.CreateName(category, EnumsFiles.Repository, "ts");

            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[] {
                string.Format("import {0} from '../Entities/{0}';", category),
                "",
                string.Format("export default interface I{0}Repository ", category) + "{",
                string.Format("\tfindById(id: number): Promise<{0}>;",category),
                string.Format("\tfind(params: any): Promise<{0}[]>;",category),
                string.Format("\tpersist(t: {0}): Promise<{0}>;", category),
                string.Format("\tdestroy(t: {0}): Promise<boolean>;",category),
                "}"
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateRepository(string category)
        {
            string path = this.createFile.CombinePath(Paths.RepositoryPath);
            string fileName = this.createFile.CreateName(category, EnumsFiles.Repository, "ts");

            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[] {
                string.Format("import {0} from '../../Domain/Entities/{0}';", category),
                "import { getRepository, Repository } from 'typeorm';",
                string.Format("import I{0}Repository from '../../Domain/Interfaces/I{0}Repository';", category),
                "import { EntityNotFound } from '../../Infraestructure/Errors/EntityNotFound';",
                "",
                string.Format("class {0}Repository implements I{0}Repository", category) + " {",
                string.Format("\tprivate repository: Repository<{0}>;", category),
                "",
                "\tconstructor() {",
                string.Format("\t\tthis.repository = getRepository({0});", category),
                "\t}",
                "",
                string.Format("\tpublic async findById(id: number): Promise<{0}> ", category) + "{",
                "\t\treturn await this.repository.findOne({ Id: id });",
                "\t}",
                "",
                string.Format("\tpublic async find(): Promise<{0}[]> ", category) + "{",
                "\t\treturn await this.repository.find();",
                "\t}",
                "",
                string.Format("\tpublic async persist(t: {0}): Promise<{0}> ", category) + "{",
                "\t\treturn await this.repository.save(t);",
                "\t}",
                "",
                string.Format("\tpublic async destroy(t: {0}): Promise<boolean> ", category) + "{",
                "\t\tconst result = await this.repository.destroy(t);",
                "\t\treturn result && result.affected === 1;",
                "\t}",
                "}",
                "",
                string.Format("export default {0}Repository", category)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateRouter(string _case)
        {
            string path = this.createFile.CombinePath(Paths.RouterPath, string.Empty);
            string fileName = this.createFile.CreateName(_case.ToLower(), EnumsFiles.Router, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = this.getData.GetContentRouter(this.name_use_case, this.category);

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateSchema(string _case)
        {
            string path = this.createFile.CombinePath(Paths.SchemaPath);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Schema, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[] {
                "import * as Joi from '@hapi/joi';",
                "export const Store"+category+"Schema = Joi.object().keys({",
                MatchInfo.GetCreateSchemaJoi(this.dataClear),
                "});",
                "export const Update"+category+"Schema = Joi.object().keys({",
                MatchInfo.GetEditSchemaJoi(this.dataClear),
                "});",
                "export const Show"+category+"Schema = Joi.object().keys({",
                MatchInfo.GetFindByIdSchemaJoi(this.dataClear),
                "});",
                "export const Index"+category+"Schema = Joi.object().keys({",
                MatchInfo.GetFindSchemaJoi(this.dataClear),
                "});",
                "export const Destroy"+category+"Schema = Joi.object().keys({",
                MatchInfo.GetDeleteSchemaJoi(this.dataClear),
                "});"
            };

            this.createFile.LoadFile(path, fileName, content);
        }
    }
}