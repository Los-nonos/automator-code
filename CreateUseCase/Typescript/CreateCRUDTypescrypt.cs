
using System;

namespace CreateUseCase
{
    class CreateCRUDTypescrypt : ICreateUseCase
    {
        private string name_use_case;
        private string category;
        private string data;
        private string base_path;

        private CreateFile createFile;
        private GetData getData;

        private const string Adapter = "Adapter";
        private const string Action = "Action";
        private const string Handler = "Handler";
        private const string Command = "Command";
        private const string Repository = "Repository";
        private const string Schema = "Schema";
        private const string Presenter = "Presenter";
        private const string Router = ".routes";

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

            string[] array = new string[] { "Create", "Edit", "Delete", "Find", "FindById" };
            foreach (var item in array)
            {
                var _data = string.Format("{0}{1}", item, name);
                this.CreateAction(_data);
                this.CreateAdapter(_data);
                this.CreateCommand(_data);
                this.CreateHandler(_data);
                this.CreatePresenter(_data);
            }
            this.CreateRouter(category);
            //this.CreateEntity(category);
            //this.CreateRepository(category);
            //this.CreateSchema(category);
        }

        private void CreateEntity(string category)
        {
            throw new NotImplementedException();
        }

        // TODO : Implement
        private void CreateRepository(string category)
        {
            throw new NotImplementedException();
        }

        private void CreatePresenter(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Presenter/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Presenter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Presenter);

            string[] content = new string[]
            {
                "import IPresenter from '../Base/IPresenter';",
                "",
                string.Format("class {0} implements IPresenter", name),
                "{",
                "\tprivate message: string;",
                "\tprivate result: any;",
                "\tconstructor(result: any, message: string) {",
                "\t\tthis.result = result;",
                "\t\tthis.message = message;",
                "\t}",
                "\tpublic toJson(): string {",
                "\t\treturn JSON.stringify(this.getData());",
                "\t}",
                "\tpublic getData(): string {",
                "\t\treturn { message: this.message, result: this.result };",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateHandler(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/Application/Handlers/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Handler, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Handler);

            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                string.Format("import {0}Command from '../../Commands/{1}/{0}Command';", _case, category),
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {}",
                "\tpublic async execute(command: " + string.Format("{0}Command", _case) + "): Promise<any> {",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        // TODO : Implement load variables
        private void CreateCommand(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/Application/Commands/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Command, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Command);

            string[] content = new string[]
            {
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateRouter(string _case)
        {
            string path = this.createFile.CombinePath("/src/routes/");
            string fileName = this.createFile.CreateName(_case.ToLower(), Router, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[]{
                "import container from '../Infraestructure/DI/inversify.config';",
                "import asyncMiddleware from '../API/Http/Middleware/AsyncMiddleware';",
                string.Format("import Create{0}Action from '../API/Http/Actions/{0}/Create{0}Action';", name_use_case),
                string.Format("import Edit{0}Action from '../API/Http/Actions/{0}/Edit{0}Action';", name_use_case),
                string.Format("import Delete{0}Action from '../API/Http/Actions/{0}/Delete{0}Action';", name_use_case),
                string.Format("import FindById{0}Action from '../API/Http/Actions/{0}/FindById{0}Action';", name_use_case),
                string.Format("import Find{0}Action from '../API/Http/Actions/{0}/Find{0}Action';", name_use_case),
                "import { Router, Request, Response, NextFunction } from 'express';",
                "import { authMiddleware } from '../API/Http/Middleware/AuthenticationMiddleware';",
                "",
                "const router = Router();",
                "",
                "router.post(",
                string.Format("\t'/{0}',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Create{0}Action>(Create{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.put(",
                string.Format("\t'/{0}/:id',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Edit{0}Action>(Edit{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.get(",
                string.Format("\t'/{0}',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Find{0}Action>(Find{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.get(",
                string.Format("\t'/{0}/:id',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<FindById{0}Action>(FindById{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.delete(",
                string.Format("\t'/{0}',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Delete{0}Action>(Delete{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "export default router;"
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        // TODO: Implement
        private void CreateSchema(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Validator/Schemas/{0}", this.category));
            string fileName = this.createFile.CreateName(name_use_case, Schema, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);
        }

        private void CreateAdapter(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Adapter/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Adapter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Adapter);
            string name_command = string.Format("{0}{1}", _case, Command);
            string name_schema = string.Format("{0}{1}", name_use_case, Schema);

            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                "import Validator from '../../Validator/Validator';",
                "import { BadRequest } from '../../Errors/BadRequest';",
                string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                string.Format("import {0} from '../../Validator/Schemas/{0}';", name_schema),
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tprivate validator: Validator;",
                "\tconstructor(@inject(Validator) validator: Validator) {",
                "\t\tthis.validator = validator;",
                "\t}",
                "\tpublic async from(req: Request): Promise<"+name_command+"> {",
                string.Format("\t\tconst error = this.validator.validate(req.body, {0});", name_schema),
                "\t\tif(error) {",
                "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                "\t\t}",
                string.Format("\t\treturn new {0}(req.body);", name_command),
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateAction(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Actions/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Action, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Action);
            string name_handler = string.Format("{0}{1}", _case, Handler);
            string name_adapter = string.Format("{0}{1}", _case, Adapter);
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