using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyAnalyzer
{
	class Program
	{
		static void Main(string[] args)
		{
			DataGenerator generator = new DataGenerator();
			DependencyAnalyser dependencyAnalyser = new DependencyAnalyser();
			dependencyAnalyser.countDependencies(generator.Logins,generator.Producers);
			//generator.printLogins(null);
			//generator.printProducers(null);
			Console.ReadKey();
		}
	}
}