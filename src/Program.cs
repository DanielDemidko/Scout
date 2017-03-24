using System;
using System.IO;
static class Program
{
	//Возвращает имя файла или директории
	private static String GetName(String Road)
	{
		if(Road[Road.Length-1]=='/'||Road[Road.Length-1]=='\\')
		{
			return Road.Remove(Road.Length-1, 1);
		}
		return Path.GetFileName(Road);
	}
	//Возвращает все содержащиеся пути
	private static String[] GetPaths(String PathInfo)
	{
		if(Directory.Exists(PathInfo))
		{
			try
			{
				String[] Directories=Directory.GetDirectories(PathInfo);
				String[] Files=Directory.GetFiles(PathInfo);
				String[] Result=new String[Directories.Length+Files.Length];
				for(Int32 i=0; i<Directories.Length; ++i)
				{
					Result[i]=Directories[i];
				}
				for(Int32 i=Directories.Length, j=0; i<Result.Length; ++i, ++j)
				{
					Result[i]=Files[j];
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}
		String[] AnyPath=new String[1];
		AnyPath[0]=PathInfo;
		return AnyPath;
	}
	//Метод вставляет файлы, использует рекурсию
	private static void PastePaths(String PasteDirectory, String[] Collection, Boolean IsCut=false)
	{
		foreach(String Road in Collection)
		{
			try
			{
				if(File.Exists(Road))
				{
					File.Copy(Road, PasteDirectory+'\\'+GetName(Road));
					if(IsCut)
					{
						File.Delete(Road);
					}
				}
				if(Directory.Exists(Road))
				{
					PastePaths(Directory.CreateDirectory(PasteDirectory+'\\'+GetName(Road)).FullName, GetPaths(Road), IsCut);
					if(IsCut)
					{
						Directory.Delete(Road);
					}
				}
			}
			catch
			{
			}
		}
	}
	//Заменяет переменные на действующие пути
	private static String[] ConvertPaths(String[] SourcePaths)
	{
		//Псевдонимы путей и их реальные аналоги
		String AppDataPath=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		const String AppDataName="%AppData%";
		String UserProfilePath=Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		const String UserProfileName="%User%";
		//Заменяем псевдонимы путей реальными путями
		String[] ResultPaths=new String[SourcePaths.Length];
		for(Int32 i=0; i<SourcePaths.Length; ++i)
		{
			//Исходный путь
			ResultPaths[i]=SourcePaths[i];
			//Работаем с исходным путем
			if(ResultPaths[i].Contains(AppDataName))
			{
				ResultPaths[i]=ResultPaths[i].Replace(AppDataName, AppDataPath);
			}
			if(ResultPaths[i].Contains(UserProfileName))
			{
				ResultPaths[i]=ResultPaths[i].Replace(UserProfileName, UserProfilePath);
			}
		}
		return ResultPaths;
	}
	private static void Main()
	{
		const String PathsFile="Paths.ini";
		const String ResultDirectory="Result";
		if(File.Exists(PathsFile))
		{
			PastePaths(ResultDirectory, ConvertPaths(File.ReadAllLines(PathsFile)), false);
		}
		else
		{
			File.WriteAllText(PathsFile, "Сниффер для Windows\nАвтор: Даниил Демидко DDemidko1@gamail.com\nЭтот файл предназначен для списка копируемых путей\nПоддерживаются: Windows XP, 7, 8, 10\nПсевдонимы путей:\n %AppData% - AppData\n %User% - Текущий пользователь\nПуть всегда копируется в папку Result - если она отсутствует, то будет автоматически создана.");
		}
	}
}