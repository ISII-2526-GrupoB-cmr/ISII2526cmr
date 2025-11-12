

-------------------------------------------------
-- MODELOS
-------------------------------------------------
SET IDENTITY_INSERT [dbo].[Models] ON
INSERT INTO [dbo].[Models] ([ID], [Name]) VALUES (1, N'Toyota Corolla');
INSERT INTO [dbo].[Models] ([ID], [Name]) VALUES (2, N'Mazda CX-5');
INSERT INTO [dbo].[Models] ([ID], [Name]) VALUES (3, N'BMW M4');
SET IDENTITY_INSERT [dbo].[Models] OFF


-------------------------------------------------
-- COCHES
-------------------------------------------------
SET IDENTITY_INSERT [dbo].[Cars] ON
INSERT INTO [dbo].[Cars]
([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceType], [Manufacturer], [PurchaseItem], [PurchasePrice], [QuantityForPurchase], [QuantityForRenting], [RentingPrice], [RimSize], [ModelID])
VALUES (1, N'Sedan', N'Gris', N'Sedán cómodo y eficiente, ideal para ciudad.', N'1.8L', N'Gasoline', N'Regular', N'Toyota', 101, 2300000, 5, 8, 85, 16, 1);

INSERT INTO [dbo].[Cars]
([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceType], [Manufacturer], [PurchaseItem], [PurchasePrice], [QuantityForPurchase], [QuantityForRenting], [RentingPrice], [RimSize], [ModelID])
VALUES (2, N'SUV', N'Azul Marino', N'SUV moderna y espaciosa con gran rendimiento.', N'2.5L', N'Diesel', N'Standard', N'Mazda', 102, 2950000, 3, 5, 120, 18, 2);

INSERT INTO [dbo].[Cars]
([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceType], [Manufacturer], [PurchaseItem], [PurchasePrice], [QuantityForPurchase], [QuantityForRenting], [RentingPrice], [RimSize], [ModelID])
VALUES (3, N'Sports', N'Rojo', N'Coupé deportivo con motor turbo de alto rendimiento.', N'3.0L Twin Turbo', N'Gasoline', N'Performance', N'BMW', 103, 6870000, 2, 2, 350, 19, 3);
SET IDENTITY_INSERT [dbo].[Cars] OFF


-------------------------------------------------
-- COMPRAS
-------------------------------------------------
SET IDENTITY_INSERT [dbo].[Purchases] ON
INSERT INTO [dbo].[Purchases] ([Id], [ApplicationUserId], [DeliveryCarLeader],  [PurchasingDate],[PaymentMethod], [PurchasingPrice])
VALUES (1, 1, N'Concesionario Madrid Centro', '2025-06-01',1 , 2300000);

INSERT INTO [dbo].[Purchases] ([Id], [ApplicationUserId], [DeliveryCarLeader],[PurchasingDate], [PaymentMethod] , [PurchasingPrice])
VALUES (2, 2, N'AutoPlanet Barcelona',  '2025-07-15',2, 2950000);

INSERT INTO [dbo].[Purchases] ([Id], [ApplicationUserId], [DeliveryCarLeader], [PurchasingDate], [PaymentMethod], [PurchasingPrice])
VALUES (3, 3, N'BMW Premium Valencia', '2025-08-10',1 , 6870000);
SET IDENTITY_INSERT [dbo].[Purchases] OFF


-------------------------------------------------
-- ITEMS DE COMPRA
-------------------------------------------------
INSERT INTO [dbo].[PurchaseItems] ([CarId], [PurchaseId], [Quantity])
VALUES (1, 1, 1);

INSERT INTO [dbo].[PurchaseItems] ([CarId], [PurchaseId], [Quantity])
VALUES (2, 2, 1);

INSERT INTO [dbo].[PurchaseItems] ([CarId], [PurchaseId], [Quantity])
VALUES (3, 3, 1);


-------------------------------------------------
-- RENTAS
-------------------------------------------------
SET IDENTITY_INSERT [dbo].[Rentals] ON
INSERT INTO [dbo].[Rentals] ([Id], [DeliveryCarDealer], [PaymentMethod], [RentignDate], [StartDate], [EndDate], [TotalPrice], [ApplicationUserId])
VALUES (1, N'MotorRent Madrid', 1, '2025-09-01', '2025-09-02', '2025-09-10', 800, 1);

INSERT INTO [dbo].[Rentals] ([Id], [DeliveryCarDealer], [PaymentMethod], [RentignDate], [StartDate], [EndDate], [TotalPrice], [ApplicationUserId])
VALUES (2, N'RentaCar BCN', 2, '2025-09-05', '2025-09-06', '2025-09-12', 900, 2);

INSERT INTO [dbo].[Rentals] ([Id], [DeliveryCarDealer], [PaymentMethod], [RentignDate], [StartDate], [EndDate], [TotalPrice], [ApplicationUserId])
VALUES (3, N'SpeedDrive Valencia', 1, '2025-09-10', '2025-09-11', '2025-09-15', 1400, 3);
SET IDENTITY_INSERT [dbo].[Rentals] OFF


-------------------------------------------------
-- ITEMS DE RENTA
-------------------------------------------------
INSERT INTO [dbo].[RentalItems] ([CarId], [RentalId], [Quantity],[Id],[RentingPrice],[Manufacturer])
VALUES (1, 1, 1,1,1,1);

INSERT INTO [dbo].[RentalItems] ([CarId], [RentalId], [Quantity],[Id],[RentingPrice],[Manufacturer])
VALUES (2, 2, 1,2,1,1);

INSERT INTO [dbo].[RentalItems] ([CarId], [RentalId], [Quantity],[Id],[RentingPrice],[Manufacturer])
VALUES (3, 3, 1,3,1,1);


-------------------------------------------------
-- RESEÑAS
-------------------------------------------------
SET IDENTITY_INSERT [dbo].[Reviews] ON
INSERT INTO [dbo].[Reviews] ([Id], [country], [created], [drivertype], [ApplicationUserId])
VALUES (1, N'España', '2025-09-15', 0, 1);

INSERT INTO [dbo].[Reviews] ([Id], [country], [created], [drivertype], [ApplicationUserId])
VALUES (2, N'España', '2025-09-20', 1, 2);

INSERT INTO [dbo].[Reviews] ([Id], [country], [created], [drivertype], [ApplicationUserId])
VALUES (3, N'España', '2025-09-25', 1, 3);
SET IDENTITY_INSERT [dbo].[Reviews] OFF


-------------------------------------------------
-- ITEMS DE RESEÑA
-------------------------------------------------
INSERT INTO [dbo].[ReviewItems] ([CarId], [ReviewId], [Description], [Rating])
VALUES (1, 1, N'Excelente coche para ciudad, muy económico.', 5);

INSERT INTO [dbo].[ReviewItems] ([CarId], [ReviewId], [Description], [Rating])
VALUES (2, 2, N'Cómodo y espacioso, ideal para viajes largos.', 4);

INSERT INTO [dbo].[ReviewItems] ([CarId], [ReviewId], [Description], [Rating])
VALUES (3, 3, N'Potencia impresionante, aunque consume mucho.', 5)