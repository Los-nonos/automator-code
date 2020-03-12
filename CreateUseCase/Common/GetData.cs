using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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

            List<DataDTO> _data = new List<DataDTO>();

            using (StreamReader r = new StreamReader("./Data/data.json"))
            {
                string json = r.ReadToEnd();
                List<DataDTO> items = JsonConvert.DeserializeObject<List<DataDTO>>(json);

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        foreach (var i in array)
                        {
                            if (item.type == i)
                            {
                                _data.Add(item);
                            }
                        }
                    }
                    return _data;
                }
                else
                {
                    throw new System.Exception("no se pudo machear la información ingresada");
                }
            }
        }

        public string[] GetContentAction(string name, string category, string name_adapter, string name_handler, string name_presenter, string name_command)
        {
            if (name.Contains("Create"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    string.Format("import Presenter from '../../Presenters/{0}/{1}';", category, name_presenter),
                    "import { success } from '../../Presenters/Base/success';",
                    "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                    string.Format("import {0} from '../../Adapters/{1}/{0}';", name_adapter, category),
                    string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../../../Domain/Entities/{0}';", category),
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
                    "\tpublic async execute(req: Request, res: Response): Promise<Response> {",
                    string.Format("\t\tconst command: {0} = await this.adapter.from({1});", name_command, "req.body"),
                    string.Format("\t\tconst response: {0} = await this.handler.execute(command);", category),
                    "\t\tconst presenter = new Presenter(response);",
                    "",
                    string.Format("\t\treturn res.status(HTTP_CODES.CREATED).json(success(presenter.getData(), '{0}: {1} created satisfully'));", name, category),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Update"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    string.Format("import Presenter from '../../Presenters/{0}/{1}';", category, name_presenter),
                    "import { success } from '../../Presenters/Base/success';",
                    "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                    string.Format("import {0} from '../../Adapters/{1}/{0}';", name_adapter, category),
                    string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../../../Domain/Entities/{0}';", category),
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
                    "\tpublic async execute(req: Request, res: Response): Promise<Response> {",
                    string.Format("\t\tconst command: {0} = await this.adapter.from({1});", name_command, "req.body, req.params"),
                    string.Format("\t\tconst response: {0} = await this.handler.execute(command);", category),
                    "\t\tconst presenter = new Presenter(response);",
                    "",
                    string.Format("\t\treturn res.status(HTTP_CODES.OK).json(success(presenter.getData(), '{0}: {1} updated satisfully'));", name, category),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Deleted"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    string.Format("import Presenter from '../../Presenters/{0}/{1}';", category, name_presenter),
                    "import { success } from '../../Presenters/Base/success';",
                    "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                    string.Format("import {0} from '../../Adapters/{1}/{0}';", name_adapter, category),
                    string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../../../Domain/Entities/{0}';", category),
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
                    "\tpublic async execute(req: Request, res: Response): Promise<Response> {",
                    string.Format("\t\tconst command: {0} = await this.adapter.from({1});", name_command, "req.params"),
                    "\t\tawait this.handler.execute(command);",
                    "",
                    "\t\treturn res.status(HTTP_CODES.NO_CONTENT).end()",
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("FindById"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    string.Format("import Presenter from '../../Presenters/{0}/{1}';", category, name_presenter),
                    "import { success } from '../../Presenters/Base/success';",
                    "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                    string.Format("import {0} from '../../Adapters/{1}/{0}';", name_adapter, category),
                    string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../../../Domain/Entities/{0}';", category),
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
                    "\tpublic async execute(req: Request, res: Response): Promise<Response> {",
                    string.Format("\t\tconst command: {0} = await this.adapter.from({1});", name_command, "req.params"),
                    string.Format("\t\tconst response: {0}[] = await this.handler.execute(command);", category),
                    "\t\tconst presenter = new Presenter(response);",
                    "",
                    string.Format("\t\treturn res.status(HTTP_CODES.OK).json(success(presenter.getData(), '{0}: {1} found'));", name, category),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    string.Format("import Presenter from '../../Presenters/{0}/{1}';", category, name_presenter),
                    "import { success } from '../../Presenters/Base/success';",
                    "import { HTTP_CODES } from '../../Enums/HttpCodes';",
                    string.Format("import {0} from '../../Adapters/{1}/{0}';", name_adapter, category),
                    string.Format("import {0} from '../../../../Application/Handlers/{1}/{0}';", name_handler, category),
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../../../Domain/Entities/{0}';", category),
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
                    "\tpublic async execute(req: Request, res: Response): Promise<Response> {",
                    string.Format("\t\tconst command: {0} = await this.adapter.from({1});", name_command, "req.body"),
                    "\t\tconst response: any = await this.handler.execute(command);",
                    "\t\tconst presenter = new Presenter(response);",
                    "",
                    string.Format("\t\treturn res.status(HTTP_CODES.OK).json(success(presenter.getData(), '{0}: {1} successfully'));", name, category),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
        }

        public string[] GetContentAdapter(string name, string name_command, string type_schema, string name_schema, string category)
        {
            string import_schema = "{ " + type_schema + " }";

            if (name.Contains("Create"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../Validator/Schemas/{1}';", import_schema, name_schema),
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(body: any): Promise<"+name_command+"> {",
                    string.Format("\t\tconst error = this.validator.validate(body, {0});", type_schema),
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(body);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Update"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../Validator/Schemas/{1}';", import_schema, name_schema),
                    "import { IdSchema } from '../../Validator/Schemas/Common';",
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(body: any, params: any): Promise<"+name_command+"> {",
                    string.Format("\t\tconst error = this.validator.validate(body, {0});", type_schema),
                    "\t\tconst errorId = this.validator.validate(params, IdSchema);",
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    "\t\tif(errorId) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(errorId)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(params.id, body);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("FindById"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    "import { IdSchema } from '../../Validator/Schemas/Common';",
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(params: any): Promise<"+name_command+"> {",
                    "\t\tconst error = this.validator.validate(params, IdSchema);",
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(params.id);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Find"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../Validator/Schemas/{1}';", import_schema, name_schema),
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(params: any): Promise<"+name_command+"> {",
                    string.Format("\t\tconst error = this.validator.validate(params, {0});", type_schema),
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(params.id);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Delete"))
            {
                return new string[]
                {
                    "import { Request, Response } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    "import { IdSchema } from '../../Validator/Schemas/Common';",
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(params: any): Promise<"+name_command+"> {",
                    "\t\tconst error = this.validator.validate(params, IdSchema);",
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(req.params.id);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else
            {
                return new string[]
                {
                    "import { Request } from 'express';",
                    "import { inject, injectable } from 'inversify';",
                    "import Validator from '../../Validator/Validator';",
                    "import { BadRequest } from '../../Errors/BadRequest';",
                    string.Format("import {0} from '../../../../Application/Commands/{1}/{0}';", name_command, category),
                    string.Format("import {0} from '../../Validator/Schemas/{1}';", import_schema, name_schema),
                    "",
                    "@injectable()",
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate validator: Validator;",
                    "\tconstructor(@inject(Validator) validator: Validator) {",
                    "\t\tthis.validator = validator;",
                    "\t}",
                    "\tpublic async from(req: Request): Promise<"+name_command+"> {",
                    string.Format("\t\tconst error = this.validator.validate(req.body, {0});", type_schema),
                    "\t\tif(error) {",
                    "\t\t\tthrow new BadRequest(JSON.stringify(this.validator.validationResult(error)));",
                    "\t\t}",
                    string.Format("\t\treturn new {0}(req.body);", name_command),
                    "\t}",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
        }

        public string[] GetContentRouter(string name, string category)
        {
            return new string[]{
                "import container from '../Infraestructure/DI/inversify.config';",
                "import asyncMiddleware from '../API/Http/Middleware/AsyncMiddleware';",
                string.Format("import Create{0}Action from '../API/Http/Actions/{0}/Create{0}Action';", name),
                string.Format("import Edit{0}Action from '../API/Http/Actions/{0}/Edit{0}Action';", name),
                string.Format("import Delete{0}Action from '../API/Http/Actions/{0}/Delete{0}Action';", name),
                string.Format("import FindById{0}Action from '../API/Http/Actions/{0}/FindById{0}Action';", name),
                string.Format("import Find{0}Action from '../API/Http/Actions/{0}/Find{0}Action';", name),
                "import { Router, Request, Response, NextFunction } from 'express';",
                "import { authMiddleware } from '../API/Http/Middleware/AuthenticationMiddleware';",
                "",
                "const router = Router();",
                "",
                "router.post(",
                string.Format("\t'/{0}',", category.ToLower()),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Create{0}Action>(Create{0}Action);", name),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.put(",
                string.Format("\t'/{0}/:id',", category.ToLower()),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Edit{0}Action>(Edit{0}Action);", name),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.get(",
                string.Format("\t'/{0}',", category.ToLower()),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Find{0}Action>(Find{0}Action);", name),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.get(",
                string.Format("\t'/{0}/:id',", category.ToLower()),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<FindById{0}Action>(FindById{0}Action);", name),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "router.delete(",
                string.Format("\t'/{0}',", category.ToLower()),
                "\t(req: Request, res: Response, next: NextFunction) => {",
                "\t\tauthMiddleware(req, res, next, ['admin']);",
                "\t},",
                "\tasyncMiddleware(async (req: Request, res: Response) => {",
                "\t\t" + string.Format("const action = container.resolve<Delete{0}Action>(Delete{0}Action);", name),
                "\t\tawait action.execute(req, res);",
                "\t}));",
                "",
                "export default router;"
            };
        }

        public string[] GetContentHandler(string name, string category, string _case)
        {
            return new string[]
            {
                "import { inject, injectable } from 'inversify';",
                string.Format("import {0}Command from '../../Commands/{1}/{0}Command';", _case, category),
                "",
                "@injectable()",
                string.Format("class {0}", name),
                "{",
                "\tconstructor() {}",
                "\tpublic async execute(command: " + string.Format("{0}Command", _case) + "): Promise<any> {",
                "\t\tthrow new Error('Method not implemented');",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };
        }

        public string[] GetContentCommand(string name, List<DataDTO> data)
        {
            if (name.Contains("Create") || (name.Contains("Find") && !name.Contains("FindById")))
            {
                return new string[]
                {
                    string.Format("class {0}", name),
                    "{",
                    MatchInfo.GetVariables(data),
                    "\tconstructor("+MatchInfo.GetParams(data)+") {",
                    "\t\t"+ MatchInfo.GetConstructor(data),
                    "\t}",
                    MatchInfo.GetFunctions(data),
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Update"))
            {
                return new string[]
                {
                    string.Format("class {0}", name),
                    "{",
                    "@private id: number",
                    "\tconstructor(id: number, " + MatchInfo.GetParams(data) + ") {",
                    "\t\tthis.id = id;",
                    "\t\t" + MatchInfo.GetConstructor(data),
                    "\t}",
                    "\tpublic getId(): number { return this.id; }",
                    MatchInfo.GetFunctions(data),
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else if (name.Contains("Delete") || name.Contains("FindById"))
            {
                return new string[]
                {
                    string.Format("class {0}", name),
                    "{",
                    "\tprivate id: number;",
                    "\tconstructor(id: number) {",
                    "\t\tthis.id = id;",
                    "\t}",
                    "\tpublic getId(): number { return this.id; }",
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
            else
            {
                return new string[]
                {
                    string.Format("class {0}", name),
                    "{",
                    MatchInfo.GetVariables(data),
                    "\tconstructor("+MatchInfo.GetParams(data)+") {",
                    "\t\t"+ MatchInfo.GetConstructor(data),
                    "\t}",
                    MatchInfo.GetFunctions(data),
                    "}",
                    "",
                    string.Format("export default {0};", name)
                };
            }
        }

        public string[] GetContentPresenter(string name)
        {
            return new string[]
            {
                "import IPresenter from '../Base/IPresenter';",
                "",
                string.Format("class {0} implements IPresenter", name),
                "{",
                "\tprivate result: any;",
                "\tconstructor(result: any) {",
                "\t\tthis.result = result;",
                "\t}",
                "\tpublic toJson(): string {",
                "\t\treturn JSON.stringify(this.getData());",
                "\t}",
                "\tpublic getData(): object {",
                "\t\tthrow new Error('Method not implemented');",
                "\t}",
                "}",
                "",
                string.Format("export default {0};", name)
            };
        }
    }
}