using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Data.EntityFramework.ExtensionMethods
{
	public static class ModelBuilderExtensions
	{
		public static PrimitivePropertyConfiguration IsUnique(this PrimitivePropertyConfiguration configuration)
		{
			return configuration.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
		}

		public static PrimitivePropertyConfiguration IsIndexed(this PrimitivePropertyConfiguration configuration)
		{
			return configuration.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
		}

		public static PrimitivePropertyConfiguration IsIndexed(this PrimitivePropertyConfiguration configuration, string namedIndex, int colNumber)
		{
			return configuration.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(namedIndex, colNumber)));
		}

	}
}
