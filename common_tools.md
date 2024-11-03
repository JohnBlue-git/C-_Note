- NLog
    
    ```csharp
    class LogSetting
    {
    	public static Logger getLogger(string path, string name)
    	{
    			var config = new LoggingConfiguration();
    
    			var fileTarget = new FileTarget {
                  Name = "logfile",
                  FileName = logPath + logName,
                  Layout = "${date:format=HH\\:mm\\:ss) [${suppercase>:${level}}] ${messager}"
          };
          var asyncFileTarget = new NLog.Targets.Wrappers.AsyncTargetWrapper(fileTarget) {
                  Name = fileTarget.Name,
                  QueueLimit = 1000,
                  OverflowAction = NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Discard)
          };
          config.AddRule(LogLevel.Trace, LogLevel.Fatal, asyncFileTarget);
    			//config.AddRule(LogLevel.Info, LogLevel.Fatal, asyncFileTarget);
    			//config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncFileTarget);
    
    // write to console at the same time
          var logConsole = new ColoredConsoleTarget();
          logConsole.Layout = "${date:format=HH\\\\:mm\\\\:ss} [${uppercase:${level}}] ${message}";
          config.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole);
    
          LogManager.Configuration = config;
    
          return LogManager.GetCurrentClassLogger();
      }
    }
    使用
    s_log = getLogger(...);
    s_log.Info(...);
    s_log.Error(...);
    
    若需要變更參數
    // NLog.config的參數
    LogManager.Configuration.Variables["basedir"] = logPath + logName;
    // Reconfigure
            var config = LogManager.Configuration;
            // method (1)
            FileTarget target = (FileTarget)config.FindTargetByName("logfile");
            if (target != null) {
                target.FileName = logPath + logName;
                LogManager.ReconfigExistingLoggers();
            }
    
    若是用設定檔
    
    Asynchronous
    Keep open
    實驗Asyn即可
    https://blog.yowko.com/nlog-async-keepfileopen/
    
    要設定alway複製 才吃得到設定檔
    
    <?xml version="1.0" encoding="utf-8" ?>
    <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
        autoReload="true"
        throwExceptions="false"
        internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">
    
        <variable name=“basedir” value=“…” />
    
        <targets aysnc=“true” aysnc-queue-limit=1000 aysnc-overflow-action=“Block”>
            <target xsi:type="File" name=“logfile”
    		fileName="${basedir}/log/${date:format=yyyy/MM/dd}/addhedge.log"
                    layout="${date:format=HH\\:mm\\:ss) [${suppercase>:${level}}] ${messager}" />
        </targets>
        <rules>
            <logger name="*" minlevel="Debug" writeTo="f" />
        </rules>
    </nlog>
    ```
    
    ![IMG_0438.PNG](https://prod-files-secure.s3.us-west-2.amazonaws.com/3c2b7db3-8976-4497-9a1d-e9182767675a/1007a15c-128b-4961-9218-91f75094be6a/IMG_0438.png)
    

- txt read write
    
    ```csharp
    https://learn.microsoft.com/zh-tw/troubleshoot/developer/visualstudio/csharp/language-compilers/read-write-text-file
    
    String line;
    try
    {
        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader("C:\\Sample.txt");
        //Read the first line of text
        line = sr.ReadLine();
        //Continue to read until you reach end of file
        while (line != null)
        {
            //write the line to console window
            Console.WriteLine(line);
            //Read the next line
            line = sr.ReadLine();
        }
        //close the file
        sr.Close();
        Console.ReadLine();
    }
    catch(Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }
    finally
    {
        Console.WriteLine("Executing finally block.");
    }
    
    try
    {
        //Pass the filepath and filename to the StreamWriter Constructor
        StreamWriter sw = new StreamWriter("C:\\Test.txt");
        //Write a line of text
        sw.WriteLine("Hello World!!");
        //Write a second line of text
        sw.WriteLine("From the StreamWriter class");
        //Close the file
        sw.Close();
    }
    catch(Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }
    finally
    {
        Console.WriteLine("Executing finally block.");
    }
    ```
    

- regex
    
    ```csharp
    using System;
    using System.Text.RegularExpressions;
    
    namespace CommonTool
    {
        public class Regexer
        {
            static public bool isPatternMatch(string pattern, string text)
            {
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                Match mat = reg.Match(text);
                return mat.Success;
            }
        }
    }
    
    參考
    https://learn.microsoft.com/zh-tw/dotnet/api/system.text.regularexpressions.regex?view=net-8.0
    
    使用範例
    @$"\bFuture_BBG_DL_{Program.date.ToString("yyyyMMdd")}_\w*.csv\b”
    public bool GetLatestFileName(FTPConnSetting conn, string path, string pattern, ref string fileName) {
    	var FileList = FTPExchanger.FTPListFileItem(conn, path);
    	foreach (var item in FileList) {
    		if (Regexer.isPatternMatch(pattern, item.Name)){
    		// to find latest
    		// for example
    		//"Future_BBG_DL_20231219_2100.csv">"Future_BBG_DL_20231219_1700.csv"
    			if (String.Compare(item.Name, fileName) > 0) {
    				fileName = item.Name;
    			}
    		}
    	}
    	return !string.IsNullOrEmpty(fileName);
    }
    ```
    

- csv helper
    
    ```csharp
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO
    using CsvHelper;
    namespace CommonTool {
    
    需要再修正 彈性化決定
    config = new CsvConfiguration(CultureInfo.InvariantCulture) {
    	HasHeaderRecord = true,
    	Delimiter = "|",
    	};
    
    public class CSVworker
    {
    	static public bool WriteCsv<T>(string path, string name, IEnumerable<T> data)
    	{
    		string csv_path = path + name;
    		try
    		{
    		using (var writer = new StreamWriter(csv_path))
    		{
    		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    		{
    		csv.WriteRecords(data);
    		}
    		}
    		}
    		catch (Exception e)
    		{
    		Console.WriteLine("[ERROR] output {0} fail; message: {1}\n", name, e.ToString());
    		return false;
    		}
    		return true;
    	}
    	static public bool ReadCsv<T>(string path, string name, bool hasHeader=true, string delimiter=",", out IEnumerable<T> data)
    	{
    		string csv_path = path + name;
    		try
    		{
    				using (var reader = new StreamReader(csv_path))
    				{
    						// skip first row
    						//reader.ReadLine();
    						var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
    		            HasHeaderRecord = hasHeader,
    		            Delimiter = delimiter, // "," "|" ...
    		        };
    		
    		        using (var csv = new CsvReader(reader, config))
    		        {
    								// .GetRecords<T>() will only work when foreach or .ToList() is used
    								// return data without ... will result in data==null 
    		            data = csv.GetRecords<T>().ToList();
    		        }
    		    }
    		}
    		catch (Exception e)
    		{
    		    data = null;
    		    Console.WriteLine("[ERROR] read {0} fail\\n", name);
    		    Console.WriteLine("[ERROR] message: {0}\\n", e.ToString());
    		    return false;
    		}
    		return true;
    	}
    }
    }
    ```
    

- FTP
    
    ```csharp
    https://github.com/robinrodricks/FluentFTP/wiki/Quick-Start-Example
    using System;
    using System.Text;
    using System.IO;
    using FluentFTP;
    //
    namespace CommonTool {
    public class FTPConnSetting
    {
    	public string IP { get; set; }
    	public string ACNT { get; set; }
    	public string PWD { get; set; }
    }
    public class FTPExchanger
    {
    	static public FtpClient SetFTP(FTPConnSetting conn)
    	{
    		DESEncoder encode = new DESEncoder();
    		FtpClient client = new FtpClient(conn.IP, conn.ACNT, encode.Decrypt(conn.PWD));
    		//client.Config...
    		//client.Config.RetryAttempts = 3;
    		return client;
    	}
    	static public IEnumerable<FtpListItem> FTPListFileItem(FTPConnSetting conn, string path)
      {
          FtpClient client = SetFTP(conn);
          List<FtpListItem> ret = new List<FtpListItem>();
          try
          {
              client.AutoConnect();
              var list = client.GetListing(path);//option: FtpListOption.Modify
              foreach (var item in list)
              {
                  if (item.Type != FtpObjectType.File) continue;
                  //else FtpObjectType.Directory 
                  ret.Add(item);
              }
              client.Disconnect();
          }
          catch (Exception e)
          {
              Console.WriteLine("[ERROR] ftp list fail\n");
          }
          return ret;
      }
    	static public bool FTPContainFile(FTPConnSetting conn, string path, string fileName)
    	{
    		FtpClient client = SetFTP(conn);
    		try
    		{
    			client.AutoConnect();
    			client.FileExists(Path.Combine(path, fileName));
    			client.Disconnect();
    			return true;
    		}
    		catch (Exception e)
    		{
    			Console.WriteLine("[ERROR] ftp not contain {0}\n", fileName);
    			return false;
    		}
    	}
    	static public bool FTPUploadFile(FTPConnSetting conn, string sourcePath, string targetPath, string fileName)
    	{
    		FtpClient client = SetFTP(conn);
    		try
    		{
    			client.AutoConnect();
    			client.UploadFile(Path.Combine(sourcePath, fileName), Path.Combine(targetPath, fileName));
    			client.Disconnect();
    			return true;
    		}
    		catch (Exception e)
    		{
    			Console.WriteLine("[ERROR] ftp upload {0} fail; message: {1}\n", fileName, e.ToString());
    			return false;
    		}
    	}
    	static public bool FTPDownloadFile(FTPConnSetting conn, string sourcePath, string targetpath, string fileName)
    	{
    		FtpClient client = SetFTP(conn);
    		try
    		{
    			client.AutoConnect();
    			client.DownloadFile(Path.Combine(targetpath, fileName), Path.Combine(sourcePath, fileName));
    			client.Disconnect();
    			return true;
    		}
    			catch (Exception e)
    		{
    			Console.WriteLine("[ERROR] ftp download {0} fail; message: {1}\n", fileName, e.ToString());
    			return false;
    		}
    	}
    }
    }
    ```
    

- DES coder
    
    ```csharp
    using System;
    using System.IO;
    using System.Text;
    using System.Security.Cryptography;
    namespace CommonTool {
    public class DESEncoder
    {
    	private string Key = "Sgo0k8dv112TCU0BiMNOo0tfR5AYmXeC";
    	private string IV = "mmVbNIIj8k4=";
    	public DESEncoder() { }
    	public string Encrypt(string encrypt)
    	{
    		string ret = "";
    		byte[] inputByteArray = Encoding.UTF8.GetBytes(encrypt);
    		using (TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider())
    		{
    			byte[] rgbKey = Convert.FromBase64String(Key);
    			byte[] rgbIV = Convert.FromBase64String(IV);
    			using (MemoryStream ms = new MemoryStream())
    	    {
            using (CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
    	      {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                ret = Convert.ToBase64String(ms.ToArray());
            }
    	    }
        }
    
        return ret;
      }
      public string Decrypt(string decrypt)
      {
          string ret = "";
          byte[] inputByteArray = Convert.FromBase64String(decrypt);
          using (TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider())
          {
              byte[] rgbKey = Convert.FromBase64String(Key);
              byte[] rgbIV = Convert.FromBase64String(IV);
              using (MemoryStream ms = new MemoryStream())
              {
                  using (CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                  {
                      cs.Write(inputByteArray, 0, inputByteArray.Length);
                      cs.FlushFinalBlock();
                      ret = Encoding.UTF8.GetString(ms.ToArray());
                  }
              }
          }
          return ret;
      }
    }
    }
    ```
    

- get resource
    
    ```csharp
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    namespace CommonTool
    {
    public class ResourceStreamer
    {
    	private static StreamReader GetStream(Encoding enEncoding, System.Reflection.Assembly assembly, string name)
    	{
    		foreach (string resName in assembly.GetManifestResourceNames())
    		{
    			if (resName.EndsWith(name))
    			{
    				return new System.IO.StreamReader(assembly.GetManifestResourceStream(resName), enEncoding);
    			}
    		}
    		return null;
    	}
    	public static string GetString(Encoding enEncoding, System.Reflection.Assembly assembly, string name)
    	{
    		StreamReader sr = GetStream(enEncoding, assembly, name);
    		string data = sr.ReadToEnd();
    		sr.Close();
    		return data;
    	}
    }
    }
    ```
    
    ```csharp
    <Project Sdk="Microsoft.NET.Sdk">
    
    <ItemGroup>
    <PackageReference Inc1ude="NLog.Config" Version="4.7.15" />
    ...
    </ItemGroup>
    
    <ItemGroup>
    <None Update="appsettings.json">
    <CopyToOutpuIDi rectory>A1ways</CopyToOuIputDirectory>
    </None>
    <None Update="appsettings.wsrd3.json">
    <CopyToOutputDi rectory>Always</CopyToOutputDirectory>
    </None>
    </ItemGroup>
    
    <ItemGroup>
    <None Remove="SQL\get_warrants.sql"></None>
    <EmbeddedResource Include="SQL\get warrants.sql">
    </ItemGroup>
    
    </Project>
    ```
    
    ```csharp
    使用
    Assembly assembly = Assembly.GetAssembly(typeof(Program));
    GetString(...UT8, assembly, name);
    ```
    

- Redis client
    
    ```csharp
    private static void Connect2Redis(List<ConfigurationOptions> opts, Fun_Ptr func, string topic) { 
            redis_set = new List<Warrant_RedisServiceTransport>();
            foreach (var opt in opts) {
                Warrant_RedisServiceTransport redis = new Warrant_RedisServiceTransport(opt, topic);
                redis.OnRedisMessage += async (redis_channel, redis_message) => ( func(redis_channel, redis_message); );
                redis_set.Add(redis);
            }
        }
        
        private static void onHandle_Message(RedisChannel redis_channel, RedisValue redis_message) {
            // Parser json message
            JObiect obj;
            string wart_stock; string wartno; string emp_no;
            try {
                obj = JObject.Parse(redis_message);
                if (obi["symbol"] = null || obj["symbol"]["string"] = null) return;
                wart_stock = obj["symbol"]["string"].ToString();
                if (obj["wartno"] = null || obj["wartno"]["string"] = null) return;
                wartno = obj["wartno"]["string"].ToString();
                if (obj["emp_no"] = null || obj["emp_no"]["string"] = null) return;
                emp_no = obj["emp_no"]["string"].ToString();
            } catch (Exception e) {
                m_gLogger.Info("Error PROXYHEDGE_ADDED_BY_GAT'EWAY: redis_message format corrupt\n" + e);
                return;
            }
            common.Fill_Info_Gap(wartno, emp_no);
            channel.Writer.WriteAsync(common);
        }
    
        /*
        Dictionary<string, Action<RedisChannel, RedisValue» onActions;
        onActions["channel"] = OnHandleAction;
        
        private static said OnHandleActions(RedisChannel channel, RedisValue message) {
            if (onActions.ContainsKey(channel.ToString()) = true)
            onActions[channel.ToString()].Invoke(channel, message);
        }
        */
    
    #
    
    using Newtonsoft.Json.Ling;
    using StackExchange.Redis;
    using System.Text.Json; 
    
    public delegate void OnReceivedRedisMessage(RedisChannel channel, RedisValue message);
    
    public delegate void TransportConnected();
        
    public interface IServiceTransport {
        bool isConnected { get; }
        void Connect_Start();
        
        IDatabase GetDataBase(int db);
        void SendByteArray(string topic, string buffer);
        void ServiceSubscribe(string topic);
        void ServiceSubscribeAsync();
        ConnectionMultiplexer GetConnectionMultiplexer(); 
        
        event OnReceivedRedisMessage OnRedisMessage;
        event TransportConnected OmServiceTransportConnected;
    }
    
    public abstract class RedisServiceTransport : IServiceTransport { 
        private ConfigurationOptions options;
        private ConnectionMultiplexer _mTransport;
        private ISubscriber _mRedisSubscriber;
        public bool isConnected => _mTransport?.IsConnected ?? false;
        
        public event TransportConnected OnServiceTransportConnected;
        private event OnReceivedRedisMessage _mOnRedisMessage;
        public event OnReceivedRedisMessage OnRedisMessage {
            add { _mOnRedisMessage += value; }
            remove { _mOnRedisMessage -= value }
        }
    
        protected RedisServiceTransport(ConfigurationOptions opt) { options = opt;
        }
        public ConnectionMultiplexer GetConnectionMultiplexer() {
            return _mTransport;
        }
        public IDatabase GetDataBase(int db) return _mTransport?.GetDatabase(db);
        }
    
        public void SendByteArray(string topic, string buffer) {
            if (isConnected = true && buffer != null && buffer.Length > 0) mRedisSubscriber.Publish(topic, buffer);
        }
        public void ServiceSubscribe(string topic) {
            if (isConnected = false) return;
            var channel = _mRedisSubscriber.Subscribe(topic);
            channel.OnMessage(message =>
                { _mOnRedisMessage?.Invoke(message.Channel, message.Message); });
        }
        public void ServiceSubscribeAsync() {
        	if (isConnected == false) return;
        	_mRedisSubscriber.Subscribe("*", (channel, msg) => ({ mOnRedisMessage?.Invoke(channel, msg); });
        }
        public void Connect_Start() {
        	if (options = null II isConnected = true) return; 
    	 try {
    	        mTransport = ConnectionMultiplexer.Connect(options);
            	if (isConnected = true) {
                	    _mRedisSubscriber = _mTransport.GetSubscriber();
                	    OnServiceTransportConnected?.Invoke();
            	}
        	} catch (RedisConnectionException ex) {}
        }
    }
    
    public class RedisService : RedisServiceTransport { 
        private string topic;
        public Warrant_RedisServiceTransport(ConfigurationOptions opt, string tpc): base(opt) { topic = tpc; }
        public void Connect() {
            base.Connect_Start();
            ServiceSubscribe(topic);
            //ServiceSubscribeAsync();// for subscribing all channel 
        }
    }
    
    service.ServiceSubscribe(“topic”);
    
    Warrant_RedisServiceTransport service = new Warrant_RedisServiceTransport(opt, topic);
     service.OnRedisMessage += func;
    ```
    

- sql query / execute / bulk insert
    
    ```csharp
    
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    
    using System.Data.SqlClient;
    using Dapper;
    using SqlBulkTools;
    
    namespace CommonTool {
    
    Type t = typeof(FutureData);
    Console.WriteLine("The {0} type has the following properties: ", t.Name);
    foreach (var prop in t.GetProperties())
        Console.WriteLine("   {0} ({1})", prop.Name,
                          prop.PropertyType.Name);
    var n = t.GetProperties().Select(x => x.Name);
    可以產生所需column
    
    sql = $"SELECT DISTINCT {...column...} FROM {...table...} WHERE {...}";
    param = _updateData.Select(x => new {
                            TransferDate = x.TransferDate,
                            DataSource = x.DataSource,
                            UnderlyingCode = x.UnderlyingCode,
                            ProductMonth = x.ProductMonth,
                            PX_LAST = x.PX_LAST,
                            CreateDate = DateTime.Now
                        });
    
    public bool DapperQuery<T>(string connStr, string sql, object? param, out IEnumerable<T> data)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                data = conn.Query<T>(sql, param);
                if (data == null)
                {
                    Console.WriteLine($"[INFO] dapper sql query nothing ...");
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            data = null;
            Console.WriteLine("[ERROR] dapper sql query fail\\n");
            Console.WriteLine("[ERROR] message: {0}\\n", e.ToString());
        }
        return data == null;
    }
    
    _updateBLPSql = "UPDATE [BLP_FuturesPrice]"
                  + " SET PX_LAST=@PX_LAST, CreateDate=@CreateDate"
                  + " WHERE TransferDate=@TransferDate AND DataSource=@DataSource AND UnderlyingCode=@UnderlyingCode AND ProductMonth=@ProductMonth";
    
    public bool DapperExecute<T>(string connStr, string sql, object? param, IEnumerable<T> data)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                bool check = conn.Execute<T>(sql, param);
                if (check == false)
                {
                    Console.WriteLine($"[ERROR] dapper sql execute fail ...");
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            data = null;
            Console.WriteLine("[ERROR] dapper sql execute error\\n");
            Console.WriteLine("[ERROR] message: {0}\\n", e.ToString());
        }
        return data == null;
    }
    
    /////////////////////////////////////////////////////////////////////////////////
    
    bulk.Setup<T>()
        .ForCollection(IEnumerable<T> data)
        .WithTable(string table)
        .AddAllColumns()
        .BulkInsertOrUpdate()
        .MatchTargetOn(x => x.TransferDate)
        .MatchTargetOn(x => x.DataSource)
        .MatchTargetOn(x => x.UnderlyingCode)
        .MatchTargetOn(x => x.ProductMonth);
        // 但是在這 Commit
    		或是
    		.ForObject(T oneItem)
        .WithTable(string table)
        .AddColumn(x => x.Symbol)
    		...
    
    public bool BulkUpdateInsert(string connStr, string table, BulkOperations bulk)
    {
        try
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    bulk.Commit(conn);
                }
                trans.Complete();
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("[ERROR] bulk sql execute error\\n");
            Console.WriteLine("[ERROR] message: {0}\\n", e.ToString());
            return false;
        }
    }
    
    }
    
    ```
    

- c# 命名規則
    
    https://learn.microsoft.com/zh-tw/dotnet/csharp/fundamentals/coding-style/identifier-names
    

- generic
    
    ```csharp
    public class MyGenericArray<T>{
            private T[] array;
            public MyGenericArray(int size)
            {
                array = new T[size + 1];
            }
            public T getItem(int index)
            {
                return array[index];
            }
            public void setItem(int index, T value)
            {
                array[index] = value;
            }
        }
    
    static void Swap<T>(ref T lhs, ref T rhs)
            {
                T temp;
                temp = lhs;
                lhs = rhs;
                rhs = temp;
            }
    ```
    

- LINQ
    
    ```csharp
    var accItem = _accData.Where(x => x.DataSource.TrimEnd() == bbgItem.DataSource.TrimEnd())
    											.Where(x => x.commodity.TrimEnd() == bbgItem.UnderlyingCode.TrimEnd());
    											.Select(x => new {Name = x.id, Double = x.num * 2})
    foreach (var item in accItem) {
    	...item.Name...
    	...item.Double...
    }
    
    first or default
    Decimal multiplyNum = _BBGData.Where(x => …).Select(x => x.Price_Multiplie).First();
    
    distinct()全欄位
    
    內部distinct
    _DL_Data.GroupBy(x => x.ParskeyeableDesSource).Select(x => x.FirstOrDefault());
    
    IEnumerable<int> squares =
        Enumerable.Range(1, 10).Select(x => x * x);
    
    "依賴LINQ的Where查詢在大量資料中反覆查詢，很容易形成效能瓶頸。面對類似需求，可透過ToDictionary()簡單轉換成Dictionary，即可獲得可觀的效能提升。"
    https://blog.darkthread.net/blog/linq-search-performance-issue/
    
    Dictionary<string, Dictionary<string, BBGData>> dict = _BBGData
        .ToDictionary(Group => Group.DataSource,
                      Group => _BBGData.Where(x => x.DataSource == Group.DataSource)
    																	 .ToDictionary(x => x.BBG_Code)
    );
    ```
    

- Func Action
    
    ```csharp
    Func<IEnumerable<string>, string> GetCommodities = delegate (IEnumerable<string> keys) {
    	return string.Join("','", keys);
    };
    使用
    _getBBGSql = $"SELECT DISTINCT [DataSource], [UnderlyingCode], [BBG_Code], [BBG_AssetClass] FROM [Futures_Ticker_List] WHERE [UnderlyingCode] IN('{GetCommodities(_accData.Keys)}')";
    ```
    

- IEnumerable<T> 應該還可以補充
    
    ```csharp
    IEnumerable<T> num;
    num = new List<T>();
    num = num.Concat(…IEnumerable<T>…)
    num = num.Append(…<T> …)
    
    ```
    

- yield return
    
    ```csharp
    private IEnumerable<FutureTickerData> MergeMethod() {
    	foreach (BBGData bbgItem in _BBGData) {
    		...
    		yield return new FutureTickerData(...);
      }
    }
    分析文
    https://blog.darkthread.net/blog/yield-return/
    ```
    

- String format
    - String format 速度還是比較慢
    
    https://blog.yowko.com/stringformat-stringconcat-stringintepolation/
    
    - 如果是從檔案讀取字串 那就只好用舊方法
    
    string s = String.Format("The temperature is {0}°C.", temp);
    
    Console.WriteLine(s);
    
    - …更人性化
    
    string bbg = BBGCode.TrimEnd();
    string asset = BBGAssetClass.TrimEnd();
    string space = (bbg.Length == 1) ? " " : string.Empty;
    …
    return $"{bbg}{space}{dateMark} {asset}";
    
    - 時間
    string fileName = "FuturePriceTicker_{DateTime. ow.ToString ("yyyyMMdd")}.csv

- date
    
    ```markdown
    date = new DateTime(YYYY, MM, dd, 13, 0, 0);
    ```
    

- tuple
    
    ```markdown
    (double Sum, int Count) t2 = (4.5, 3);
    ```
    

- convert
    
    ```markdown
    Int32.Parse(item.Name.Substring(23, 4));
    ```
    

- &= |= 流程控制 全部 只要有一個
    
    ```csharp
    bool isQueryAcc = false;
    foreach (var DB in AppSetting.ServerACC.GetDBListSplit()) {
    	bool current = method.TryQueryCommodity(AppSetting.ServerACC.GetConn(DB.DBName).ConnectionString, DB.dataSource);
    	isQueryAcc |= current;
    }
    if (isQueryAcc == false) return;
    ```
    

- Task 進化到如果可以不用semaphore
    
    ```csharp
    using System;
    using System.Threading.Tasks;
    //void ActFun()
    Action act = ActFun;
    //anonomous
    Action<object> action = (object obj) => {
           Console.WriteLine("Task={0}, obj={1}, Thread={2}",
           Task.CurrentId, obj, Thread.CurrentThread.ManagedThreadId);
    };
    Task t1 = new Task(action, "alpha");
    t1.Start();
    t1.Wait();
    // return parameter
    Task<bool> isTask = Task.Run(() => { ...; return true; });
    isTask.Wait();
    // async await
    Task accountTask = Task.Run(async () => await GetAccounts(sz));
    Task depositsTask = Task.Run(async() => Console.WriteLine(await GetDeposits(sz)));
    Task.WhenAll(accountTask, depositsTask);
    ...
    static public async Task<int> GetDeposits(int sz) {
        var count = 0;
        for (var i = 0; i < sz; i++) count += i;
        await ...如果途中非同步(就可以async await 而回傳不見得要用await接)...
        return count;
    }
    請參考
    https://learn.microsoft.com/zh-tw/dotnet/csharp/asynchronous-programming/async-return-types
    
    using System;
    using System.Threading.Tasks;
    Semaphore bbg = new Semaphore(initialCount: 0, maximumCount: 1); bbg.Release();
    Task upload = Task.Run(() => {
    	bbg.WaitOne();...如果還是要用semaphore...
    	...這裡盡量簡潔...
    }
    Task read = Task.Run(() => {
    	Task.WaitAll(upload);...Task.WaitAll(...List<Task>.ToList()...);
    	...這裡盡量簡潔...
    }
    ```
    

- Appsetting … read … json
    
    ```csharp
    第一種
    appSetting.json
    {
      "warrant2": {
        "DataSource": "128.110.233.123",
        "InitialCatalog": "warrant2",
        "UserID": "apowner",
        "Password": "ZkTloBvbo+E.", 
        "IntegratedSecurity": false,
        "PersistSecuritylnfo": true,
        "ConnectTimeout": 100,
        "Tables": {
          "warrant_marketwart":"warrant_marketwart",
          "warrant_addhedge_autoparameter": "warrant_addhedge_autoparameter",
          "warrant_hegtool": "warrant_hegtool",
          "warrant_autohedge_batch_arg": "warrant_autohedge_batch_arg"
        }
        "MultipleActiveResultSets": true, 
        "isTestEnv": false 
    }
    
    AppSetting.cs
    using System.Net;
    using StackExchange.Redis;
    using Microsoft.Extensions.Configuration;
    using Dapper;
    using DBAdapter;
    public class SQL_Connection {
        public string DataSource( get; set; }
        public string InitialCatalog ( get; set; }
        public string UserID ( get; set; }
        public string Password get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool PersistSecuritylnfo ( get; set; }
        public int ConnectTimeout { get; set; }
        
        public abstract class Abstract_Tables {}
        
        protected SqlConnectionStringBuilder getConn() {
            var cs = new Sq1ConnectionStringBuilder();
            cs.DataSource = DataSource;
            cs.InitialCatalog = InitialCatalog;
            cs.UserID = UserID;
            var decrypt = new TripleDESEncoder();//密碼工具去找一下
            cs.Password = decrypt.Decrypt(Password);//密碼工具去找一下
            cs.IntegratedSecurity = IntegratedSecurity;
            cs.PersistSecuritylnfo = PersistSecuritylnfo;
            cs.ConnectTimeout = ConnectTimeout;
            return cs;
        }
    }
    public class Warrant2_Connection : SQL_Connection {
        public Tables tables { get; set; }
        public bool MultipleActiveResultSets { get; set }
        
        public class Tables : Abstract_Tables {
            public string warrant_marketwart { get; set; }
            public string warrant_hegtool { get; set; }
        }
        
        public SelannectionStringBuilder getConn_Warrant() {
            var cs = base.getConn();
            return cs;
        }
    }
    public class Redis_Conneclion {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public List<ConfigurationOptions> configs;
        ...
    }
    public class AppSetting {
        static public Warrant2Sonnection warrant2 { get; set; }
        static public User_DB_Connection User_DB { get; set; }
        static public Redis_Connection redis { get; set; }
        
        static AppSetting() {
            Configuration configuration new ConfigurationBuilder() .AddJsonFile("appsettings.json", optional: false, reloadCmChange: true)
                .AddJsonFile($"appSettings.{Dns.GetHostName()}.json", true, true)
                .Build();
            return configuration.Get<AppSetting>();
        }
    }
    
    第二種
    {
        "Tables":  "A|a, B|b, C|c, ..."
    }   
    List<Pair> GetDBListSplit() {
    	var sep = Tables.Replace(" ", "").Split(',');
    	List<Pair> ret;
    	foreach (var item in sep) {
    		string[] arr = item.Split('|');
    		ret.Add(new Pair(arr[0], arr[1]));
    	}
    	return ret;
    }
    AppSetting.ServerACC.GetDBListSplit()
    
    ```
    

- set project path
    
    ```csharp
    private static bool isValidPath(string path) {
    if (false=string.IsNullOrEmpty(path)&&Directory.Exists(path))return true;
    else return false;
    }
    
    projectPath = AppDomain.CurrentDomain.BaseDirectory;
    
    當預設路徑 類似(../../)
    projectPath = 
    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
    .Parent.Parent.FullName;
    ```
    

- get argument
    
    ```csharp
    method 1
    public class CommandLineOptions {
    	[Option('f',"file", Required = false, HelpText = "path")]
      public string path { get; set; }
    }
    string[] arg;// -f /ap/sts/...
    string res;
    Parser.Default.ParseArguments<CommandLineOptions>(args)
          .WithParsed(opts => res = opts.path);
    
    method 2
    string[] args = Environment.GetCommandLineArgs();
    for (int L = 1; L < args.Length; L++) {
    	switch (args[L])
      {
    	  case "-P":
    	    ...do something...
          break;
        default:
          break;
       }
    }
    
    ```
    

- exception … class and function
    
    ```csharp
    取出exception 但是給直接相關的class跟method
    如果是sql就是回套件...名稱
    var st = new StackTrace(e, true);
    var frame = st.GetFrame(0);
    var className = frame.GetMethod().DeclaringType.FullName;
    var methodName = frame.GetMethod().Name;
    
    目前測試可行
    class = this.name;
    method = System.Reflection.MethodBase.GetCurrentMethod().Name;
    ```
    

 

- addHedge參考結構
    
    ```csharp
    class Program {
        public static Logger m_gLogger;
        private static string m_strWarrantServerPath;
        private static Warrant_Info_Center info_center; 
        private static ListsWarrant_RedisServiceTransport> redis_set;
        private delegate void Fun_Ptr(RedisChannel redis_channel, RedisValue redis_message);
        private static Channel<Warrant_Information> channel;
        // How to use Channe1,15 //https,//ieremyhytes.blogspot.com/2021/02/an-introduction-to-channels-in-c.html
        
    // Main
    
        static async Task Main(stringli cogs) {
            // Logger
            SettingLog(project_path, @"/addhedge.log");
            // Initialize
            info_centor = new Warrant_Info_fenter(AppSetting.User_DB.  AppSetting.Warrant2);
            // Creat information channel (productor consumer datastructure)
            channel = Channel.CreateUnbounded<WarrantInformation>();
            // Connect to Redis
            Cbmiect2Redis(ApaSetting.redis.getConnRedis(), onHandle_Message, "PROXYHEDGEADDEDBY_GATEWAY");
            // Write to DB
            Task tsk = Write2DB(…);
            await tsk;
        }
    
    使用channel...producer comsumer...
    private static async Task Write2DB(Warrant2_Connection Warrant2) {
            string Conn_Warrant2 = Warrant2.getConn_Warrant2().ConnectionString;
            while (await channel.Reader.WaitToReadAsync()) {
                if (channel.Reader.TryRead(out Warrant_Information common) = false) continue;
                try {
                    var bulk = new BulkOperations();
                    using (TransactionScope trans = new TransactionScope()) {
                            using (SqlConnection conn = new SqlConnection(Conn_Warrant2)) {
                                    if (info_center_TryGet_hegetool(common.wart_stock, out Warrant_Hedgetool hedge)) {
                                        bulk.Setup<Warrant_Hedgetool> .ForObject(hedge)
                                        .WithTable(Warrant2.tables.warrant_hegtool)
                                        .AddAllColumns()
                                        .Upsert()
                                        .MatchTarget0m(x => x.wart_no)
                                        .MatchTarget0m(x => x.commodity)
                                        .Commit(conn);
                                    }
                            }
                    }
                } catch() {
                    Thread.Sleep(100);
                    channel.Writer.WriteAsync(common); //可以awiat也可以不要
                }
            }
    ```
