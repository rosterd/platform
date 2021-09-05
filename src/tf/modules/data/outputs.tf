output "search_id" {
  value = azurerm_search_service.search.id
}

output "instrumentation_key" {
  value = azurerm_application_insights.insights.instrumentation_key
}

output "app_id" {
  value = azurerm_application_insights.insights.app_id
}

output "sql_server_id" {
  value = azurerm_mssql_server.sqlserver.id
}

output "sql_database_id" {
  value = azurerm_mssql_database.sqldatabase.id
}
