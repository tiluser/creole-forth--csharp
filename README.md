# creole-forth--csharp
Creole Forth for C#
-------------------

Intro
-----

This is a Forth-like scripting language built in C# and is preceded by similar languages that were built in
Delphi/Lazarus, Excel VBA, JavaScript, and Python.  It can be used either standalone or as a DSL embedded as 
part of a larger application. 

Methodology
-----------
Primitives are defined as C# methods attached to objects. They are roughly analagous to core words defined 
in assembly language in some Forth compilers. They are then passed to the buildPrimitive method in the 
CreoleForthBundle class, which assigns them a name, vocabulary, and integer token value, which is used as an 
address. 

High-level or colon definitions are assemblages of primitives and previously defined high-level definitions.
They are defined by the colon compiler. 

Notes: 

(1) Interpreter is working ok for now, compiler isn't yet. So all definitions currently have to be defined
as primitives to use them.

(2) Just have source code files so as to remain as version-neutral as possible. Program.cs builds the primitives
and uses them in a console app. 
