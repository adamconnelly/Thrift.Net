# Thrift.Net

[![Build Status](https://dev.azure.com/adamrpconnelly/Thrift.Net/_apis/build/status/Build%20and%20Run%20Tests?branchName=main)](https://dev.azure.com/adamrpconnelly/Thrift.Net/_build/latest?definitionId=3&branchName=main)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/adamrpconnelly/Thrift.Net/3)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/adamrpconnelly/Thrift.Net/3)

[![Total alerts](https://img.shields.io/lgtm/alerts/g/adamconnelly/Thrift.Net.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/adamconnelly/Thrift.Net/alerts/)
[![Language grade: C#](https://img.shields.io/lgtm/grade/csharp/g/adamconnelly/Thrift.Net.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/adamconnelly/Thrift.Net/context:csharp)

The aim of the Thrift.Net project is to create an implementation of the
[Thrift](https://thrift.apache.org/) compiler and runtime library in C#,
providing a first class experience for .NET developers wanting to use Thrift.

## License

Thrift.Net is provided under the MIT license, which you can find
[here](LICENSE).

## Project Status

At the moment the project is **_not ready for production use_**. We are
currently working on implementing a Thrift compiler that is compatible with the
[official Apache runtime library](https://www.nuget.org/packages/ApacheThrift).
Once we have a working compiler, the next step will be to implement the runtime
library to make Thrift.Net self contained.

- For information about the initial design goals for the project, see the
  [design goals](docs/design-goals.md) page.
- For information on the project roadmap, see the
  [roadmap page](docs/roadmap.md).
- For information about the compiler design, see the
  [compilation page](docs/compilation.md).

## Contributing

If you want to contribute to the development of Thrift.Net, please see our
[contribution guide](docs/CONTRIBUTING.md).
