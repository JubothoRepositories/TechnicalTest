Hello,

This is Svetoslav Mitov and those are my implementation notes.

I am using
 - Visual Studio 2019. For c# I prefer it over Visual Studio code
 - c# 8 "Nullable and not-nullable reference types"  https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references
 - the new switch expression, again from c# 8, no need to - but it's really cool
 - strict validation rules (with fluent validation), I try to never allow on object to exist in an invalid state, which saves tons of worries and further validations after creations
 - read-only domain objects, which again - saves tons of worries and further validations after creations

 - the repository pattern, which allows me to separates the data access logic and the business logic.
 - Microsoft.Extensions.DependencyInjection for Dependency Injection
 - some elements of the DDD Rich Domain Model, which means I will have cross refferences between the aggregate root and the objects belonging to the same aggregate
 - sealed for class when I don't think someone would like to inherit it. I beleave that the defautl should be sealed and 'unseal' or something should exist to explicitly mark when you want a class to be inheritable

I am not using
 - async/await because the contract in the assignment is synchronous
 - logging, but I could add it very easy with DI again - nlog or serilog, but not log4net, it's so not elegant
 - more than one implementation of service/repository. but I could add more implementations very easy, for example ef core in memory (https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory)



First, I start by creating a blank solution. This is possible since VS 2017, in the earlier versions an additional template was required in order to create Blank Solution, which is so useful.
Then this ReadMe.txt is created in which I am putting all (politically correct) thoughts.

A .net core 3.0 console project is created in which I will test all stuff I wrote as a code.
There is 3.1 already, but I don't need it, we will be just fine with 3.0.

I am making a .net core 3.0 library, which will hold the logic, services, repositories and so on.
It could be in the same console project, but a library would allow an easy integration with REST service.
Repositories, implementations, etc, could be in different libraries as well, but the scope of the project is small and even a demo does not worth splitting in multiple libraries.
Generally, decoupling is always desirable.

Note! In the specification from the provided document for Buy a double is used. 
It would be more appropriate to use decimal instead of double because double is floating *binary* point types while decimal is floating *decimal* point type.
According to Microsoft decimal is appropriate for financial and monetary calculations.
However I am sticking with double, because this is what is in the task, there may be considerations for using it for which I am not aware, for example third party integrations, another components requiring double and not decimal, etc, in real life situation I would discuss this.

Next the interfaces are created, about the INameQuantity, it's not really clear what should contains Quantity, but the most obvious would be to contains the AVALIABLE copies of a book, which would show the user how many books are there.
Note! It's a good practice for the methods to have verb or verb phrase in it's name. For example not just Quantity, but GetQuantity. You don't trust me? OKay, go ask Robert C. Martin :)

Note! I am adding and using the "Recommended Tags for Documentation Comments" for all public methods, classes and properties. This helps in generating documentation and while coding as well.

=== Structure 
There is the Domain Layer, which will contain all Domain logic.
I will use Rich Domain Model, which means almost all of the business logic will be there.
The rest will be in the Services (one - Store), which is in the Application Layer.

Note! I am huge fan of readonly and Immutable structs, which means that all members are read-only.
This helps a lot in protecting the values  from unintended modifications outside of my control AND does not require re-validate everything again on each mutation.
Note! I am using fluent validations in the constructor of the domain objects. This let me never ever end up a model in an invalid state.

IReadOnlyCollection and IReadOnlyList are faster than IEnumerable, where the access is continuous (you get only enumerator) and you don't have random element access, which is slow in terms of counting and searching.
Often people are using IEnumerable while sometimes they should prefer ICollection or IReadOnlyCollection or IList or IReadOnlyList

I am using newtonsoft json schema, however it's not free, there are free packages, but the 1000 validations per hour here should not be an issue.