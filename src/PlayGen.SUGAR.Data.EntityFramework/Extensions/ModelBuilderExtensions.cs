using System.Collections.Generic;

//namespace PlayGen.SUGAR.Data.EntityFramework.ExtensionMethods
//{
//	public static class ModelBuilderExtensions
//	{
//		private static readonly Dictionary<PrimitivePropertyConfiguration, IndexAnnotation> IndexAnnotations = new Dictionary<PrimitivePropertyConfiguration, IndexAnnotation>();

//		private static PrimitivePropertyConfiguration SetIndex(this PrimitivePropertyConfiguration configuration, IndexAttribute indexAttribute)
//		{
//			IndexAnnotation indexAnnotation;
//			if (IndexAnnotations.TryGetValue(configuration, out indexAnnotation))
//			{
//				indexAnnotation = new IndexAnnotation(indexAnnotation.Indexes.Concat(new[] { indexAttribute }).ToArray());
//				IndexAnnotations[configuration] = indexAnnotation;
//			}
//			else
//			{
//				indexAnnotation = new IndexAnnotation(indexAttribute);
//				IndexAnnotations.Add(configuration, indexAnnotation);
//			}
//			configuration.HasColumnAnnotation("Index", indexAnnotation);
//			return configuration;
//		}


//		public static PrimitivePropertyConfiguration IsUnique(this PrimitivePropertyConfiguration configuration)
//		{
//			return configuration.SetIndex(new IndexAttribute { IsUnique = true });
//		}

//		public static PrimitivePropertyConfiguration IsIndexed(this PrimitivePropertyConfiguration configuration)
//		{
//			return configuration.SetIndex(new IndexAttribute());
//		}

//		public static PrimitivePropertyConfiguration IsIndexed(this PrimitivePropertyConfiguration configuration, string namedIndex, int colNumber)
//		{
//			return configuration.SetIndex(new IndexAttribute(namedIndex, colNumber));
//		}

//	}
//}

