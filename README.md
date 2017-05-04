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
  Compilation time: 1007 ms  Execution time: 1120 ms
 Use startup profile:
  Compilation time: 694 ms  Execution time: 785 ms
  Compilation time: 612 ms  Execution time: 714 ms
  Compilation time: 634 ms  Execution time: 726 ms

== Run Roslyn Compiler with .NET Framework 4.6.1 ==
 Creating startup profile:
  Compilation time: 961 ms  Execution time: 1069 ms
 Use startup profile:
  Compilation time: 646 ms  Execution time: 740 ms
  Compilation time: 645 ms  Execution time: 741 ms
  Compilation time: 605 ms  Execution time: 699 ms

== Run Roslyn Compiler with .NET Framework 4.6.2 ==
 Creating startup profile:
  Compilation time: 2223 ms  Execution time: 2346 ms
 Use startup profile:
  Compilation time: 1395 ms  Execution time: 1510 ms
  Compilation time: 1406 ms  Execution time: 1518 ms
  Compilation time: 1390 ms  Execution time: 1500 ms

== Run Roslyn Compiler with .NET Framework 4.7 ==
 Creating startup profile:
  Compilation time: 2199 ms  Execution time: 2325 ms
 Use startup profile:
  Compilation time: 1437 ms  Execution time: 1546 ms
  Compilation time: 1367 ms  Execution time: 1485 ms
  Compilation time: 1355 ms  Execution time: 1465 ms
  ```

As you can see it's much slower with .NET framework 4.6.2 and 4.7.

## Remarks

- I tried UseLegacyJIT in app.config but it resulted in worse performance (as we could have expected)
```xml
<runtime>
      <useLegacyJit enabled="1" />
 </runtime>
 ```
 - Tracing with WPA confirmed me the JIT time is really slower with 4.6.2
 ![diff_screen](Diff_Screen.png)
 
