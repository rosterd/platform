output "admin_app_service_id" {
  value = azurerm_app_service.admin.id
}

output "admin_app_service_app_settings" {
  value = azurerm_app_service.admin.app_settings
}

output "client_app_service_id" {
  value = azurerm_app_service.client.id
}

output "client_app_service_app_settings" {
  value = azurerm_app_service.client.app_settings
}