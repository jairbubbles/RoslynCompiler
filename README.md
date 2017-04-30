# RoslynCompiler
Simple RoslynCompiler benchmark. I'm currently switching a project to Roslyn and startup performance (mainly due to JIT compilation) is an issue on that tool (compared to previous compiler). I've tried several versions of Roslyn and I have inconsistent results. Right now I'm focusing on benchmarking performances for different .NET Framework versions.

I'm using multicore JIT to improve startup time.

On this sample I'm using latest [Roslyn NuGet package](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp/) (currently 2.1.0) 

# Instructions
Open RoslynComparer.sln with Visual Studio 2017 and launch Comparer project.

Here are the results on my computer:
```
== Run Roslyn Compiler with .NET Framework 4.6 ==
 Creating startup profile:
  Compilation time: 1106 ms  Execution time: 1228 ms
  Compilation time: 1092 ms  Execution time: 1189 ms
  Compilation time: 1148 ms  Execution time: 1425 ms
 Use startup profile:
  Compilation time: 707 ms  Execution time: 805 ms
  Compilation time: 738 ms  Execution time: 832 ms
  Compilation time: 683 ms  Execution time: 781 ms

== Run Roslyn Compiler with .NET Framework 4.6.1 ==
 Creating startup profile:
  Compilation time: 1080 ms  Execution time: 1192 ms
  Compilation time: 1048 ms  Execution time: 1144 ms
  Compilation time: 1034 ms  Execution time: 1129 ms
 Use startup profile:
  Compilation time: 665 ms  Execution time: 759 ms
  Compilation time: 664 ms  Execution time: 760 ms
  Compilation time: 633 ms  Execution time: 734 ms

== Run Roslyn Compiler with .NET Framework 4.6.2 ==
 Creating startup profile:
  Compilation time: 2386 ms  Execution time: 2514 ms
  Compilation time: 2308 ms  Execution time: 2422 ms
  Compilation time: 2290 ms  Execution time: 2406 ms
 Use startup profile:
  Compilation time: 1491 ms  Execution time: 1604 ms
  Compilation time: 1479 ms  Execution time: 1592 ms
  Compilation time: 1460 ms  Execution time: 1573 ms
  ```

As you can see it's much slower with framework 4.6.2

## Remarks

- I tried UseLegacyJIT in app.config but it resulted in worse performance (as we could expect)
