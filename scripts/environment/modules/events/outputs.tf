output "function_app_id" {
  value = azurerm_function_app.funcapp.id
}

output "function_app_settings" {
  value = azurerm_function_app.funcapp.app_settings
}

output "function_app_connection" {
  value = azurerm_function_app.funcapp.connection_string
}

output "azurerm_eventgrid_topic_id" {
  value = azurerm_eventgrid_topic.events.id
}

output "azurerm_eventgrid_topic_connection" {
  value = azurerm_eventgrid_topic.events.endpoint
}