using FluentMigrator;

namespace ProdutosMvc.Migrations
{
	[Migration(202502070001)]
	public class Migration_070202025_Inicial : Migration
	{
		public override void Up()
		{
			Create.Table("Produtos")
				.WithColumn("Id").AsInt32().PrimaryKey().Identity()
				.WithColumn("Nome").AsString(100).NotNullable()
				.WithColumn("Descricao").AsString(200).Nullable()
				.WithColumn("Preco").AsDecimal().NotNullable()
				.WithColumn("DataCadastro").AsDateTime().NotNullable();
		}

		public override void Down()
		{
			Delete.Table("Produtos");
		}
	}
}
