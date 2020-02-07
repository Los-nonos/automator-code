
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
            //this.CreateRepository(category);
            //this.CreateSchema(category);
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

            // TODO: Modificate content for presenter
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
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {}",
                "\tpublic async execute(command: any): Promise<any> {",
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
                "import { Request, Response } from 'express';",
                "import { injectable } from 'inversify';",
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };
        }

        private void CreateRouter(string _case)
        {
            string path = this.createFile.CombinePath("/src/routes/");
            string fileName = this.createFile.CreateName(_case, Router, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[]{
                "import container from '../Infraestructure/DI/inversify.config",
                "import asyncMiddleware from '../API/Http/Middleware/AsyncMiddleware';",
                string.Format("import Create{0}Action from '../API/Http/Actions/Create{0}Action';", name_use_case),
                string.Format("import Edit{0}Action from '../API/Http/Actions/Edit{0}Action';", name_use_case),
                string.Format("import Delete{0}Action from '../API/Http/Actions/Delete{0}Action';", name_use_case),
                string.Format("import FindOne{0}Action from '../API/Http/Actions/FindOne{0}Action';", name_use_case),
                string.Format("import FindAll{0}Action from '../API/Http/Actions/FindAll{0}Action';", name_use_case),
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
                "\t\t" + string.Format("const action = containter.resolve<Create{0}Action>(Create{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}",
                "",
                "router.put(",
                string.Format("\t'/{0}/:id',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = containter.resolve<Edit{0}Action>(Edit{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}",
                "",
                "router.get(",
                string.Format("\t'/{0}',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = containter.resolve<FindAll{0}Action>(FindAll{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}",
                "",
                "router.get(",
                string.Format("\t'/{0}/:id',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = containter.resolve<Find{0}Action>(Find{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}",
                "",
                "router.delete(",
                string.Format("\t'/{0}',", category),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = containter.resolve<Delete{0}Action>(Delete{0}Action);", name_use_case),
                "\t\tawait action.execute(req, res);",
                "\t}",
                "",
                "export default router;"
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        // TODO: Implement
        private void CreateSchema(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Validator/Schemas/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Schema, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);
        }

        private void CreateAdapter(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Adapter/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Adapter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Action);
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

        private void CreateAction(string _case)
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Actions/{0}", this.category));
            string fileName = this.createFile.CreateName(_case, Action, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, Action);
            string[] content = new string[]
            {
                "import { Request, Response } from 'express';",
                "import { inject, injectable } from 'inversify';",
                "import Presenter from '../../Presenters/null';",
                "import { success } from '../../Presenters/Base/success';",
                "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tprivate adapter: any;",
                "\tprivate handler: any;",
                "\tconstructor(@inject(any) adapter: any, @inject(any) handler: any) {",
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