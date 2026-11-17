# Hướng dẫn khởi tạo dự án
# Tài khoản 
```
Admin
user: admin
pass: admin123
--------
User
user: test
pass: test123
```
###### Database name tạo = "MyStore"
```

USE [MyStore]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](max) NOT NULL,
	[CustomerPhone] [nvarchar](15) NOT NULL,
	[CustomerEmail] [nvarchar](max) NULL,
	[CustomerAddress] [nvarchar](max) NULL,
	[Username] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [nvarchar](max) NULL,
	[AddressDelivery] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[ProductName] [nvarchar](max) NOT NULL,
	[ProductDescription] [nvarchar](max) NOT NULL,
	[ProductPrice] [decimal](18, 2) NOT NULL,
	[ProductImage] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/17/2026 11:27:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Username] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[UserRole] [nchar](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2002, N'Điện thoại')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2003, N'Laptop')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2004, N'Tai nghe')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2005, N'Đồng hồ')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2006, N'Máy tính')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2007, N'Tivi')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2008, N'Linh kiện')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2009, N'Loa')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2010, N'Máy chơi Game')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (1, N'Quản trị viên', N'0123456789', N'admin@mystore.com', N'Hồ Chí Minh', N'admin')
INSERT [dbo].[Customer] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [CustomerAddress], [Username]) VALUES (2, N'Phong Test', N'09999999999', N'pt123@gmail.com', N'abc,TP.Hcm', N'test')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [AddressDelivery]) VALUES (1, 1, CAST(N'2026-11-14T20:14:39.573' AS DateTime), CAST(1250000.00 AS Decimal(18, 2)), N'Đã giao', N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PaymentStatus], [AddressDelivery]) VALUES (2, 2, CAST(N'2026-11-14T20:38:49.240' AS DateTime), CAST(234970000.00 AS Decimal(18, 2)), N'Đang giao hàng', N'abc,TP.Hcm')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (1, 4007, 1, 1, CAST(1250000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (2, 4016, 2, 1, CAST(178990000.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderDetail] ([ID], [ProductID], [OrderID], [Quantity], [UnitPrice]) VALUES (3, 4005, 2, 2, CAST(27990000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4002, 2002, N'iPhone 18 Pro 256GB', N'Màn hình: OLED Super Retina XDR 6,3 inch, ProMotion 120Hz, độ sáng tối đa 3.000 nit. Hiệu năng: Chip A20 Pro tiến trình 2nm, GPU 7 lõi, hỗ trợ AI. Camera: Bộ 3 camera sau 48MP, zoom quang học 4x, quay video 4K Dolby Vision 120 fps; camera trước 18MP. Pin và sạc: Xem video đến 34 giờ, sạc 50% trong khoảng 15 phút. Thiết kế: Khung nhôm nguyên khối, Ceramic Shield 2, kháng nước IP68, trọng lượng 211g. Kết nối và tính năng: 5G, Wi-Fi 7, Bluetooth 6, Face ID, Apple Intelligence hỗ trợ tiếng Việt.', CAST(38690000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/i/p/iphone-18-pro-01.jpg')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4003, 2002, N'Samsung Galaxy S26 Ultra 5G 12GB 256GB', N'Hiệu năng vượt trội với Snapdragon 8 Elite Gen 5 For Galaxy, RAM 12GB xử lý đa nhiệm mượt mà. Camera 200MP kết hợp zoom 100x, OIS và quay 8K cho trải nghiệm chụp quay chuyên nghiệp. Màn hình Dynamic AMOLED 2X 6.9 inch, độ phân giải 2K+ hiển thị sắc nét, mượt mà. Pin 5000mAh hỗ trợ sạc nhanh 60W, sạc không dây và sạc ngược tiện lợi. Thiết kế nguyên khối Armor Aluminum, kháng nước IP68, hỗ trợ S Pen và nhiều tính năng AI.', CAST(30490000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/s/a/samsung-galaxy-s26-ultra-12.jpg')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4004, 2003, N'MacBook Air M5', N'Loại CPU: Chip Apple M5. Dung lượng RAM: 16GB. Ổ cứng: 512GB. Loại card đồ họa: GPU 8 lõi. Màn hình: Liquid Retina, Độ sáng 500 nit, Hỗ trợ một tỷ màu, Dải màu rộng (P3), Công nghệ True Tone, Hỗ trợ lên đến hai màn hình ngoài.', CAST(33390000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/0/1/01_6_4.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4005, 2003, N'Laptop ASUS TUF Gaming A15', N'Laptop ASUS TUF Gaming A15 FA506NCG-HN329W là dòng laptop gaming bền bỉ nổi bật với thiết kế chuẩn quân đội MIL-STD-810H, mang lại độ cứng cáp ấn tượng. AMD Ryzen 7 8845HS và GeForce RTX 3050 đủ sức xử lý mượt game AAA và tác vụ đồ họa. Màn hình 15.6 inch Full HD 144Hz giảm thiểu hiện tượng mờ nhòe.', CAST(27990000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/s/s/sssxs_69.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4006, 2004, N'Apple AirPods 5', N'Chống ồn chủ động ANC mạnh hơn đến 1,5 lần. Âm Thanh Thích Ứng. Tự cân bằng ANC và Xuyên Âm theo môi trường. Siri AI & Dịch Trực Tiếp. Trợ lý rảnh tay, hỗ trợ giao tiếp đa ngôn ngữ. Bền bỉ chuẩn IP57. Kháng bụi, mồ hôi và nước tốt hơn. Pin đến 20 giờ với hộp sạc, 4 giờ khi bật ANC, sạc nhanh 5 phút ≈ 1 giờ, USB-C.', CAST(37900000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/a/p/apple-airpods-5-1.jpg')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4007, 2004, N'True Wireless JBL Wave Beam 2', N'Tận hưởng thời gian nghe nhạc đến 10 giờ và thêm 30 giờ từ hộp sạc kèm sạc nhanh. Chỉ với 10 phút khi sạc bạn sẽ có ngay 3 giờ để tận hưởng tiếp (Trong trường hợp tắt ANC). Được trang bị chuẩn kháng bụi và nước IP54, JBL Wave Beam 2 sẵn sàng cùng bạn chơi nhạc mà "không sợ mưa rơi". Với ứng dụng JBL Headphones, bạn có thể dễ dàng lựa chọn EQ phù hợp bạn - hoặc tự tùy chỉnh theo ý muốn.', CAST(1250000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/t/a/tai-nghe-khong-day-jbl-wave-beam-2_17_.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4008, 2005, N'Apple Watch Series 12', N'Công nghệ màn hình: Màn hình Retina OLED LTPO3 luôn bật, tần số quét 1Hz. Đường kính mặt: 42 mm. Kích thước cổ tay phù hợp: Vừa với cổ tay 15 - 20 cm. GPS tích hợp: GPS tích hợp sẵn trên đồng hồ. Tương thích: iPhone 11 trở lên, bao gồm iPhone SE (thế hệ thứ 2 trở lên), sử dụng iOS 27 trở lên. Thời lượng pin: 24 giờ (Chế độ nguồn điện thấp: 38 giờ, Tập luyện ngoài trời: 10 giờ). Tiện ích sức khoẻ: Chế độ luyện tập, Theo dõi giấc ngủ, Đo nhịp tim, Đo lượng oxy trong máu, Đếm bước chân, Tính calo tiêu thụ, Tính quãng đường chạy, Đo mức độ stress, Cảm biến nhiệt độ, Phát hiện té ngã, Cảnh báo nhịp tim bất thường, Đo điện tâm đồ.', CAST(9990000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/2/0/20250719_1.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4009, 2005, N'Samsung Galaxy Watch8', N'Công nghệ Super AMOLED với độ sáng 3000 nits và kích thước 1.34 inch mang lại hình ảnh sắc nét và sống động. Chất liệu nhôm nguyên khối và kính sapphire tạo nên sự chắc chắn, nhẹ nhàng và tinh tế cho người dùng. Hỗ trợ đo điện tâm đồ, huyết áp, nồng độ oxy trong máu (SpO2), cùng theo dõi giấc ngủ và mức độ stress, giúp bạn quản lý sức khỏe hiệu quả. Khả năng chống nước 10 ATM hoàn hảo cho bơi lội và lặn vùng nước nông mà không lo ngại về độ bền. Tích hợp esim và các tính năng điều khiển chụp ảnh, nghe nhạc, cùng trợ lý ảo, mang lại trải nghiệm sử dụng tiện lợi và hiện đại.', CAST(6890000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/s/m/sm-l320_4_.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4010, 2007, N'Smart Tivi Samsung UHD 4K', N'Hình ảnh 4K sắc nét và sống động với bộ xử lý Crystal Processor 4K và HDR, mang đến trải nghiệm xem tuyệt vời. Âm thanh sống động với công nghệ OTS Lite, kết hợp hoàn hảo cùng Q-Symphony để nâng cao trải nghiệm giải trí. Hệ điều hành Tizen thông minh giúp truy cập nhanh chóng nhiều ứng dụng và tính năng giải trí hấp dẫn. Thiết kế nhỏ gọn với viền mỏng, lý tưởng cho không gian sống hạn chế mà vẫn giữ được vẻ đẹp sang trọng.', CAST(6990000.00 AS Decimal(18, 2)), N'https://cellphones.com.vn/media/catalog/product/t/v/tv-ss-43u8000f-uhd-4k-43_2_.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4011, 2007, N'Smart tivi Samsung QLED 4K', N'Công nghệ Quantum Dot mang đến 100% dải màu, cho trải nghiệm hình ảnh sống động và chân thực ở mọi mức độ sáng. Kích thước màn hình 75 inch, công nghệ nâng cấp hình ảnh mạnh mẽ giúp mọi nội dung yêu thích được trình chiếu ở chuẩn 4K sắc nét. Công nghệ Quantum HDR tạo ra khung hình rực rỡ với sắc đen sâu thẳm và sắc trắng thuần khiết. Công nghệ Adaptive Sound và Object Tracking Sound Lite mang đến âm thanh sống động, đồng bộ hoàn hảo với hình ảnh. Thiết kế MetalStream tinh tế và chân đế điều chỉnh đa năng cho phép tùy chỉnh vị trí TV, nâng cao trải nghiệm giải trí.', CAST(10590000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/s/m/smart-tivi-samsung-qled-65q6fa-4k-65-inch_4_.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4012, 2010, N'Sony Playstation 5 Bundle Marvel', N'Máy chơi game Sony PlayStation 5 Bundle Marvels Spiderman 2 Limited Edition sở hữu cấu hình đột phá nhờ được trang bị CPU mạnh mẽ. Cùng với đó là kiến trúc đồ hoạ với sức mạnh đáng kinh ngạc, hỗ trợ ray-tracing và xuất hình ảnh game độ phân giải cao. Đặc biệt, thế hệ Play Station này còn bao gồm bộ nhớ trong 825GB, hứa hẹn đem lại trải nghiệm gaming mượt mà, đỉnh cao.', CAST(19990000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/m/a/may-choi-game-sony-playstation-5-bundle-marvels-spiderman-2-limited-edition-_2.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4013, 2010, N'ROG Xbox Ally X', N'Bộ xử lý AMD Ryzen AI Z2 Extreme cùng NPU AI mạnh mẽ, chiến mượt mọi siêu phẩm AAA. Thiết kế công thái học với tay cầm bo cong phong cách Xbox, thoải mái kiểm soát từng trận đấu. Tinh hoa Xbox hòa quyện cùng sức mạnh ROG, nhỏ gọn nhưng bùng nổ hiệu năng ở mọi nơi. Windows 11 tối ưu cho handheld, truy cập trực tiếp toàn bộ thư viện game, chơi mọi lúc mọi nơi. Màn hình FHD 120Hz hỗ trợ FreeSync™ Premium, hiển thị siêu mượt và sắc nét từng khung hình.', CAST(27490000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/r/o/rog-xbox-ally-x.jpg')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4014, 2006, N'PC CPS Gaming G13 i7 12700', N'PC G13 được chế tác từ linh kiện chất lượng cao. Cấu hình đủ đáp ứng cho những tựa game mạnh. Hỗ trợ card màn hình RTX 5060 cho việc đồ họa. Thiết kế case gọn nhẹ phù hợp đặt mọi không gian.', CAST(32590000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/p/c/pc-cps-gaming-g11-02_2.jpg')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4015, 2006, N'PC CPS Gaming G06 i5 12400F', N'PC G6 được chế tác từ linh kiện chất lượng cao. Cấu hình đủ đáp ứng cho những tựa game mạnh. Hỗ trợ card màn hình RX 7600 cho việc chơi game. Thiết kế case gọn nhẹ phù hợp đặt mọi không gian.', CAST(20690000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/p/c/pc-cps-gaming-g6_1.png')
INSERT [dbo].[Product] ([ProductID], [CategoryID], [ProductName], [ProductDescription], [ProductPrice], [ProductImage]) VALUES (4016, 2008, N'VGA MSI Geforce RTX 5090', N'VGA MSI GeForce RTX 5090 32G Ventus 3X OC nổi bật với bộ nhớ GDDR7 32GB, kiến trúc NVIDIA Blackwell và thiết kế ba quạt TORX FAN 5.0 tản nhiệt mạnh mẽ. Card hỗ trợ PCIe 5.0, DisplayPort 2.1 và HDMI 2.1a, đáp ứng tốt nhu cầu hiển thị cao cấp. Mẫu VGA MSI này có thể tùy chỉnh linh hoạt qua MSI center để ép xung hay quan sát các thông số khác.', CAST(178990000.00 AS Decimal(18, 2)), N'https://cdn2.cellphones.com.vn/x/media/catalog/product/v/g/vga-msi-geforce-rtx-5090-32g-ventus-3x-oc_1_.png')
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'admin', N'$2a$11$B11IMcJGJi10uITqtISZ/uKYu8ZQYW73FcE9rV4RlO9Ra2Jn6ZS5i', N'A')
INSERT [dbo].[User] ([Username], [Password], [UserRole]) VALUES (N'test', N'$2a$11$Q5DrpRdqLGeXJU8P39uNNu4QBszOxW0ukjJ03PE/2qLqB6qD0p9TK', N'C')
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_User_Customer] FOREIGN KEY([Username])
REFERENCES [dbo].[User] ([Username])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_User_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Pro_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Pro_Category]
GO

```

Cảm ơn đã ngó đến dòng này, chúc bạn một này tốt lành, Hết.
