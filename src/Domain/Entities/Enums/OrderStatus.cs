using System.ComponentModel;

namespace Domain.Entities.Enums;

public enum OrderStatus
{
	None = 0,
	Creating = 1,
	Received = 3,
	SentToProduction = 4
}