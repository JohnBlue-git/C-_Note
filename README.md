
### c# data type
![C# DATA TYPE](https://github.com/user-attachments/assets/66550534-fc87-4f67-abe3-e6ae087e6ea1)

### Modfier
- Public
- internal (public but the scope restricted to the same assembly files)
- protected
- private

### Reflection
- Is a tool to detect components in the assembly or dll
- It can use to “unit test” or do “reverse engineering”

### anonymous
- A type that deosn’t defined its class name
```c#
var anonymousData = new
{
	ForeName = "Jignesh",
	SurName = "Trivedi"
};
```

### 比較運算子(==)跟 Equals()方法的差異
- == 運算子跟 Equals()方法都是用來比較兩個數值型別資料項目或是參考型別的資料項目，等號比較運算子(==)是比較運算子
- 而 Equals()方法比較字串的內容

### Some reference
https://job.achi.idv.tw/2020/11/26/most-asked-c-interview-questions/

### Try
```c#
int.TryParse(line, out int number)
```

### Property : field and function
```c#
public class Student
{
    public string Name { get; set; }
    public double GPA { get; set; }
}

var students = new List<Student>
{
    new Student { Name = "Alice", GPA = 3.6 },
    new Student { Name = "Bob", GPA = 3.2 },
    new Student { Name = "Charlie", GPA = 3.8 }
};
```

### List and String
```c#
.Length
.Add
.AddRange
.Remove
.RemoveAt
.RemoveAt(.Count - 1);

https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.add?view=net-8.0
```

### LINQ
is extension of IEnumerable<T>
```c#
using System.Linq;

if(sortAscending) {
    qry = qry.OrderBy(x=>x.Property);
} else {
    qry = qry.OrderByDescending(x=>x.Property);
}

https://learn.microsoft.com/zh-tw/dotnet/csharp/linq/get-started/introduction-to-linq-queries
```
查询表达式语法（Query Expression）和方法语法（Fluent Syntax）
```c#
    // Group
        IEnumerable<string> list = new List<string> {
            "aaa.frx", "bbb.TXT", "xyz.dbf", "abc.pdf", "aaaa.PDF", "xyz.frt", "abc.xml", "ccc.txt", "zzz.txt"
        };
        var res = list
            .GroupBy(x => x.Substring(x.Length - 3, 3).ToLower())
            .Select(x => new {
                name = x.Key,
                count = x.Count(),
            });
        foreach (var item in res) {
            Console.WriteLine($"{item.name}:{item.count}");
        }
        Console.WriteLine("");
        
    // OrderBy
        IEnumerable<string> list = new List<string>{"abc", "bc", "c"};
        IEnumerable<string> res = list.OrderBy(x => x.Length);
        foreach (var item in res) {
            Console.Write(item);
        }
        Console.WriteLine("");


    // ToDictionary
	.ToDictionary(
	    x => x.Key
	    , x => x.Sum(xv => xv.Age)
	); 
```
.First() and .FirstOrDefault()
```c#
```


### Dapper (extension)
ORM，英文叫Object Relational Mapping，翻譯成中文為物件關聯對映。 ORM 在網站開發結構中，是在『資料庫』和『 Model 資料容器』兩者之間，
```c#
using Dapper;

.AsList();
```

### Task
``` c#
// Task
Task hello ＝new Task(() => { … });
hello.Start();
hello.Wait();
也可await hello;

// 回傳Task + Run
Task hello() { return Task.Run(() => { …; Thread.Sleep(3000); }); }
hello().Wait();

// 回傳 接收
td = hello(); 回傳是 Task<T>
td.Result; 有使用 會自動await

// 回傳接收 Async Await
// 可以type
int num = await hello(); 

// 修飾 async Task 跟 await
await hello;
await hello();

// 一定要有async 不然要回傳Run
async Task hello() { ...; return; }
async Task<T> hello() { ...; return T; }

// async Task<T> 內一定要有 Task.Run 或 其他async Task
// 不然使用起來跟同步一樣了
async Task<T> hello() { await Task.Run(…); }
```

### Implementing a Singleton Pattern

Task:
- Implement a thread-safe Singleton class in C#.

Constraints:
- The Singleton class should be designed in such a way that only a single instance of the class can exist in the application, and this instance should be accessible globally.

```c#
public sealed class Singleton
{
    private static Singleton instance = null;
    private static readonly object padlock = new object();
    Singleton() {}
    public static Singleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
```

