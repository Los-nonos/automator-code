using System;
using CreateUseCase;

namespace CreateArchitecture
{
    class CreateBaseArchitecture
    {
        private string base_path;
        private CreateFile createFile;
        public CreateBaseArchitecture(string path)
        {
            this.base_path = path;
            this.createFile = new CreateFile(null, path);
        }

        public void execute(string type_architecture)
        {
            if (type_architecture == TypesArchitectures.HEXAGONAL)
            {
                CreateMiddlewares();
                CreateErrors();
                CreateEnums();
                CreateServer();
                CreateServices();
                CreateUtils();
                CreateFolders();
            }
            else if (type_architecture == TypesArchitectures.COMMON)
            {
                throw new System.NotImplementedException();
            }
            else
            {
                throw new System.Exception("No se ingresó un tipo de arquitectura valido!");
            }
        }

        private void CreateUtils()
        {

        }

        private void CreateServices()
        {
            string path = this.createFile.CombinePath("/src/API/Http/Validator");
            string fileName = this.createFile.CreateName("Validator", "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[]{
                "import * as Joi from 'joi';",
                "import { injectable } from 'inversify';",
                "",
                "@injectable()",
                "class Validator implements IValidator {",
                "\t\tpublic validate(data: any, schema: any) {",
                "\t\t\tconst validationsOptions = { abortEarly: false, allowUnknown: true };",
                "\t\t\tconst { error } = Joi.validate(data, schema, validationsOptions);",
                "",
                "\t\t\treturn error;",
                "\t\t}",
                "",
                "\tpublic validationResult(details: any) {",
                "\t\tconst usefulErrors: any = {",
                "\t\t\terrors: {},",
                "\t\t\ttype: 'UnprocessableEntity',",
                "\t\t};",
                "",
                "\t\tdetails.map((error: any) => {",
                "\t\t\tif (error.type === 'E0001') {",
                "\t\t\t\tusefulErrors.type = 'BadRequestException';",
                "\t\t\t}",
                "",
                "\t\t\tif (!usefulErrors.errors.hasOwnProperty(error.path.join('_'))) {",
                "\t\t\t\tusefulErrors.errors[error.path.join('_')] = {",
                "\t\t\t\t\tfield: error.path.join('_'),",
                "\t\t\t\t\ttype: error.type,",
                "\t\t\t\t\tmessage: error.message,",
                "\t\t\t\t};",
                "\t\t\t}",
                "\t\t});",
                "",
                "\t\treturn usefulErrors;",
                "\t}",
                "}",
                "",
                "export default Validator;",
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateServer()
        {
            string path = this.createFile.CombinePath("/src");
            string fileName = this.createFile.CreateName("index", "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] contentIndex = new string[] {
                "import 'reflect-metadata';",
                "import { Express } from 'express';",
                "import express = require('express');",
                "import Router from './Infrastructure/Router/Router';",
                "",
                "class App {",
                "\tprivate express: Express;",
                "\tprivate router: Router;",
                "",
                "\tconstructor() {",
                "\t\tthis.express = express();",
                "\t\tthis.router = new Router(this.express);",
                "\t}",
                "\tpublic run() {",
                "\t\tprocess",
                "\t\t\t.on('unhandledRejection', (reason, p) => {",
                "\t\t\t\tconsole.error(reason, 'Unhandled Rejection at Promise', p);",
                "\t\t\t})",
                "\t\t\t.on('uncaughtException', err => {",
                "\t\t\t\tconsole.error(err, 'Uncaught Exception thrown');",
                "\t\t\t\tprocess.exit(1);",
                "\t\t\t});",
                "",
                "\t\tconst port = parseInt(process.env.API_PORT, 10) || 3001;",
                "\t\tthis.upServer(port);",
                "\t\tthis.router.up();",
                "\t}",
                "",
                "\tprivate upServer(port: number) {",
                "\t\tthis.express.listen(port, () => {",
                "\t\t\tconsole.log(`Server is run in port ${port}`);",
                "\t\t});",
                "\t}",
                "}",
                "",
                "const app = new App();",
                "app.run();"
            };

            this.createFile.LoadFile(path, fileName, contentIndex);

            path = this.createFile.CombinePath("/src/Infrastructure/", "Router");
            fileName = this.createFile.CreateName("Router", "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] contentRouter = new string[] {
                "import { Application } from 'express';",
                "import bodyParser = require('body-parser');",
                "import cors = require('cors');",
                "import routes from '../../routes/index.routes';",
                "import * as dotenv from 'dotenv';",
                "import container from '../DI/inversify.config';",
                "import ErrorHandler from '../utils/ErrorHandler';",
                "import { createConnectionDB } from '../Database/Configs';",
                "import { ServerError } from '../Errors/ServerError';",
                "",
                "class Router {",
                "\tprivate express: Application;",
                "",
                "\tconstructor(express: Application) {",
                "\t\tthis.express = express;",
                "\t}",
                "",
                "\tpublic async up() {",
                "\t\tconst result = dotenv.config();",
                "\t\tif (result.error) {",
                "\t\t\tthrow new ServerError(`Not found dotenv`);",
                "\t\t}",
                "",
                "\t\tawait createConnectionDB();",
                "\t\tthis.middlewares();",
                "\t\tthis.userRoutes();",
                "\t\tthis.catchErrors();",
                "\t}",
                "",
                "\tpublic getApp(): Application {",
                "\t\treturn this.express;",
                "\t}",
                "",
                "\tprivate middlewares() {",
                "\t\tthis.express.use(cors());",
                "\t\tthis.express.use(bodyParser.urlencoded({ extended: false }));",
                "\t\tthis.express.use(bodyParser.json());",
                "\t}",
                "",
                "\tprivate userRoutes() {",
                "\t\tthis.express.use('/apiv1', routes);",
                "\t}",
                "",
                "\tprivate catchErrors() {",
                "\t\tconst errorHandler: ErrorHandler = container.get(ErrorHandler);",
                "\t\tthis.express.use(errorHandler.logger);",
                "\t\tthis.express.use(errorHandler.execute);",
                "\t}",
                "}",
                "",
                "export default Router;",
            };

            this.createFile.LoadFile(path, fileName, contentRouter);
        }

        private void CreateFolders()
        {

        }

        private void CreateEnums()
        {
            string path = this.createFile.CombinePath("/src/API/Http/Enums");
            string fileName = this.createFile.CreateName("HttpCodes", "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = new string[] {
                "export const HTTP_CODES = {",
                "\tOK: 200,",
                "\tCREATED: 201,",
                "\tASYNC: 202,",
                "\tNO_CONTENT: 204,",
                "\tBAD_REQUEST: 400,",
                "\tUNAUTHORIZED: 401,",
                "\tFORBIDDEN: 403,",
                "\tNOT_FOUND: 404,",
                "\tMETHOD_NOT_ALLOWED: 405,",
                "\tGONE: 410,",
                "\tUNPROCESSABLE_ENTITY: 422,",
                "\tINTERNAL_ERROR: 500,",
                "\tUNAVAILABLE: 503,",
                "};",
            };

            this.createFile.LoadFile(path, fileName, content);
        }

        private void CreateErrors()
        {
            CreateHttpErrors();
            CreateInfrastructureErrors();
        }

        private void CreateInfrastructureErrors()
        {

        }

        private void CreateHttpErrors()
        {

        }

        private void CreateMiddlewares()
        {

        }
    }
}