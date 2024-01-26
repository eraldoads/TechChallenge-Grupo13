using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class CreateStatusPagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamento_Pedido_PedidoIdPedido",
                table: "Pagamento");

            migrationBuilder.DropIndex(
                name: "IX_Pagamento_PedidoIdPedido",
                table: "Pagamento");

            migrationBuilder.DropColumn(
                name: "PedidoIdPedido",
                table: "Pagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_IdPedido",
                table: "Pagamento",
                column: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamento_Pedido_IdPedido",
                table: "Pagamento",
                column: "IdPedido",
                principalTable: "Pedido",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamento_Pedido_IdPedido",
                table: "Pagamento");

            migrationBuilder.DropIndex(
                name: "IX_Pagamento_IdPedido",
                table: "Pagamento");

            migrationBuilder.AddColumn<int>(
                name: "PedidoIdPedido",
                table: "Pagamento",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_PedidoIdPedido",
                table: "Pagamento",
                column: "PedidoIdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamento_Pedido_PedidoIdPedido",
                table: "Pagamento",
                column: "PedidoIdPedido",
                principalTable: "Pedido",
                principalColumn: "IdPedido");
        }
    }
}
