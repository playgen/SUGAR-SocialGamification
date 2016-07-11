# SUGAR Social Gamification Platform
SUGAR provides gamification elements for increased user engagement and for bringing players together to cooperate. The APIs and functionalities are designed from the ground up to facilitate a large variety of individual and team play dynamics. From sharing goals and achievements to sharing resources and inventories, SUGAR makes it possible to turn a one-person experience into a team experience.

#### This repository is updated from the previous URL of : https://github.com/playgenhub/rage-sga-server/


## Overview
This software allow developers to layer game mechanics affording game-inspired social relations and interactions on top a system to support engagement, collaboration, and learning. Two main forms of social interaction are supported: player-player interactions (such as matches) and group interactions (such as shared team goals or team vs. team competitions).

## Architecture
SUGAR Server is built with `ASP.NET Core` and `MVC 7` which is an open-source and cross-platform framework for building modern cloud-based Web applications using ASP.NET 5. The aim is to provide an optimized development framework which can be deployed to the cloud or run on-premises. It consists of modular components with minimal overhead, to retain flexibility while constructing and extending more features. SGA’s ASP.NET 5 component is cross-platform which runs on Windows, Mac and Linux.

SUGAR is capable to runs on top both `Internet Information Server (IIS)` (Windows only solution) & `Kestrel` web server (cross-platform).

`Entity Framework 6` is an object relational mapping (ORM) library, that is capable of mapping SGA classes to database entities (a.k.a tables). EF6 also eases the process of writing migrations when new features are introduced or changes have been made to existing schemas. Underlying layer of data storage uses `MySQL 5.6` or `MariaDB 10.1` database.

### Code Structure
* `/Controllers` - Contains Code for All RESTful APIs
* `/DAL` - Database Access Layer & Data Initilisation.
* `/Models` - Database storage strcutures & classes
* `/Policies` - Security Policy repositories
* `/Tests` - Various Unit & Integration Tests
* `config.json` - Base configuration file
* `config.development.json` - Overrides config.json, used for Development environment
* `config.staging.json` - Overrides config.json, used for Staging environment
* `config.production.json` - Overrides config.json, used for Production environment
* `project.json` - Stores various project configurations such as BUILD processes & libraries,
* `Startup.cs` - Application entry point

## Usage
* Download or Clone the repository.
* Download & Install either [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx) or [Visual Studio Code](https://code.visualstudio.com/)
* Open Project (`SocialGamificationAsset.sln`) in Visual Studio.
* Start using or extending the code

## Installation

### Development
Follow all the guidelines as mentioned in Usage section,
1. Open Visual Studio
2. Browse `config.development.json` (create a  new file if it doesn't exists)
3. Copy content rom `config.json` file & paste it in `config.development.json` file
4. Change `Data.MySQLConnection.ConnectionString` details as per your requirements.

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Changelog

See [CHANGELOG](CHANGELOG.md)


## [License](LICENSE.md)

**APACHE LICENSE 2.0**

**Copyright (C) 2016 PlayGen Ltd**

Contact : contact@playgen.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
