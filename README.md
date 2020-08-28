# Unity Fantasma

The Unity editor extension to generate stub-files for your C# classes.  
The main purpose for this editor extension is to generate stub-files from external DLLs for CI and testing without having to manage the SDK(s) and other components in a repository.  
This editor extension generates C# classes only for public members.

> NOTICE: THIS PROJECT IS VERY EXPERIMENTAL. PLEASE USE IT AT YOUR OWN RISK.

## Requirements

- Unity 2018.4.20f1
- Roslyn Analyzer

## Installation

Please add the following section to the top of the package manifest file (`Packages/manifest.json`).  
If the package manifest file already has a `scopedRegistries` section, it will be added there.

```json
{
  "scopedRegistries": [
    {
      "name": "Mochizuki",
      "url": "https://registry.npmjs.com",
      "scopes": ["moe.mochizuki"]
    }
  ]
}
```

And the following line to the `dependencies` section.

```json
"moe.mochizuki.fantasma": "VERSION"
```

## Basic Usage

Open `Mochizuki/Fantasma/Editor` from the Unity menubar.  
Set the top-level class library and destination directory, then press the button to generate it.  
When you provided `Assembly.A.dll` with its dependency `Assembly.B.dll`, Fantasma makes below directories and files.

```
ROOT_DIRECTORY (YOU PROVIDED)
+- Assembly.A
|  |- Assembly.A.asmdef
|  |- Path/To/Namespace/ClassA.cs
+- Assembly.B
|  |- Assembly.B.asmdef
|  |- Path/To/Namespace/ClassB.cs
```

## How it works

It dynamically reads the target assemblies and its dependencies, enumerates all public members via reflection API, and generates classes based on that information.  
As a result, information that is present in the code but is missing at the time of the IL code, or the body of a method or property, is not implemented.

Examples of missing information on IL:

- `new` keyword in nested class, methods and others.
- `virtual` keyword in override methods and properties.
- `unsafe` keyword in class, methods and others.

Examples of missing information on Reflection:

- `base` keyword in constructor.
  - So, Fantasma will generate a `protected internal` default constructor for all classes.

## License

MIT by [@MikazukiFuyuno](https://twitter.com/MikazukiFuyuno) and [@6jz](https://twitter.com/6jz)
