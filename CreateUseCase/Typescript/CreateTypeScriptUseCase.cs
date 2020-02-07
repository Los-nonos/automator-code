using System;
using System.Collections.Generic;
using System.IO;

namespace CreateUseCase
{
    class CreateTypeScriptUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private string name_use_case;
        private string data;
        private string category;
        private GetData getData;
        private CreateFile createFile;

        const string Adapter = "Adapter";
        const string Command = "Command";
        const string Handler = "Handler";
        const string Action = "Action";
        const string Presenter = "Presenter";
        const string Schema = "Schema";
        #endregion
        public CreateTypeScriptUseCase(string init_path)
        {
            this.init_path = init_path;
            this.getData = new GetData();
        }

        public void Execute(string name, string data, string category)
        {
            this.name_use_case = name;
            this.category = category;
            this.data = data;
            this.createFile = new CreateFile(name, init_path);
            //CreatePresenter();
            CreateHandler();
            CreateAdapter();
            CreateAction();
            CreateCommand();
        }

        private void CreatePresenter()
        {
            
        }

        private void CreateAdapter()
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Adapter/{0}", this.category));
            string fileName = this.createFile.CreateName(Adapter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = this.createFile.CreateName(Adapter);
            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                "import Validator from '../../Validator/Validator';",
                "import { BadRequest } from '../../Errors/BadRequest';",
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tprivate validator: Validator;",
                "\tconstructor(@inject(Validator) validator: Validator) {",
                "\t\tthis.validator = validator;",
                "\t}",
                "\tpublic async from(req: Request, res: Response): Promise<any> {",
                "\t\tconst error = this.validator.validate(req.body, null);",
                "\t\tif(error) {",
                "\t\t\tthrow new BadRequest(JSON.stringfy(this.validator.validationResult(error)));",
                "\t\t}",
                "\t\treturn new Object(req.body);",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateCommand()
        {
            string path = this.createFile.CombinePath(string.Format("/src/Application/Commands/{0}", category));
            string name_file = this.createFile.CreateName(Command, "ts");
            this.createFile.VerificateFileOrCreate(path, name_file);
            path = Path.Combine(path, name_file);

            string name = this.createFile.CreateName(Command);

            List<DataDTO> info = this.getData.ClearData(this.data);
            List<string> _content = new List<string>() {
                "class " + name + " {",
                "\t" + MatchInfo.GetVariables (info),
                "\tconstructor(" + MatchInfo.GetParams (info) + ") {",
                "\t" + MatchInfo.GetConstructor (info),
                "\t}",
                "\t" + MatchInfo.GetFunctions (info),
                "}",
                "export default " + name + ";"
            };
            this.createFile.LoadFile(path, name, _content.ToArray());
        }

        private void CreateHandler()
        {
            string path = this.createFile.CombinePath(string.Format("/src/Application/Handlers/{0}", this.category));
            string fileName = this.createFile.CreateName(Handler, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = this.createFile.CreateName(Handler);

            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                string.Format("import {0}Command from '../../Commands/{1}/{0}Command';", name_use_case, category),
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {}",
                "\tpublic async execute(command: " + string.Format("{0}Command", name_use_case) + "): Promise<any> {",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateAction()
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Actions/{0}", this.category));
            string fileName = this.createFile.CreateName(Action, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = this.createFile.CreateName(Action);
            string name_handler = string.Format("{0}{1}", name_use_case, Handler);
            string name_adapter = string.Format("{0}{1}", name_use_case, Adapter);
            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                "import Presenter from '../../Presenters/null';",
                "import { success } from '../../Presenters/Base/success';",
                "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                string.Format("import {0} from '../../Adapter/{1}/{0}';", name_adapter, category),
                string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                string.Format("\tprivate adapter: {0};", name_adapter),
                string.Format("\tprivate handler: {0};", name_handler),
                "\tconstructor("+string.Format("@inject({0}) adapter: {0}, @inject({1}) handler: {1}", name_adapter, name_handler)+") {",
                "\t\tthis.adapter = adapter;",
                "\t\tthis.handler = handler;",
                "\t}",
                "\tpublic async execute(req: Request, res: Response) {",
                "\t\tconst command: any = this.adapter.from(req);",
                "\t\tconst response: any = await this.handler.execute(command);",
                "\t\tconst presenter = new Presenter(response);",
                "",
                "\t\tres.status(HTTP_CODES.OK).json(success(presenter.getData(), null));",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }
    }
}