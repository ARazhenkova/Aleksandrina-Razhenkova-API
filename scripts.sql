DROP TABLE IF EXISTS EMPLOYEES
GO

CREATE TABLE EMPLOYEES
(
	[ID]					BIGINT			NOT NULL IDENTITY(1,1),
	[ROW_VERSION]			ROWVERSION,
	[UID]					CHAR(36)		NOT NULL,
	[NAME]					NVARCHAR(128)	NOT NULL,
	[LAST_NAME]				NVARCHAR(128)	NOT NULL,
	[EMAIL]					NVARCHAR(128)	NOT NULL,
	[COUNTRY_ALPHA_2_CODE]	CHAR(2)			NOT NULL,
	[BIRTH_DATE]			DATETIME		NOT NULL,
	[DEPARTMENT_NAME]		NVARCHAR(64)	NOT NULL,
	[POSITION_NAME]			NVARCHAR(64)	NOT NULL,
	[ACCESS_LEVEL]			INT				NOT NULL,
	[PASSWORD]				NVARCHAR(512)	NOT NULL,
	[SALT]					NVARCHAR(64)	NOT NULL,

	CONSTRAINT [PK_EMPLOYEES_ID] PRIMARY KEY CLUSTERED(ID),
	CONSTRAINT [UI_EMPLOYEES_UID] UNIQUE(UID),
	CONSTRAINT [UI_EMPLOYEES_EMAIL] UNIQUE(EMAIL)
)
GO

INSERT INTO EMPLOYEES
	  (UID										, NAME			, LAST_NAME		, EMAIL					, COUNTRY_ALPHA_2_CODE	, BIRTH_DATE	, DEPARTMENT_NAME	, POSITION_NAME	, ACCESS_LEVEL	, SALT							, PASSWORD)
VALUES('26E81B98-BB10-4906-A854-927557BC2DB8'	, N'Иван'		, N'Иванов'		, 'ivan@gmail.com'		, 'BG'					, '1990-12-10'	, N'Отдел 1'		, N'Позиция 1'	, 0				, 'vazc7XreSj/DE/2pfPIzoA=='	, 'Ty8I0q98ep7JHpFjtaG46N2OuiXZS9qlFDcotnMVDzxKkRlLA5NBz3TfbPSuh+1qmYqCe6Ajo7rPNKugTGQCj32h4na0i5ff00LIZTSmDyOhUhYpnCC5780ffZmT4SoiUKi+76EkRrHZ87IuWACng7JCTODEc9ueCoPAnNMiLY0=')
	, ('4CE3A934-AC94-4C7C-941B-ADF9FAA501FE'	, N'Петър'		, N'Петров'		, 'petar@gmail.com'		, 'BG'					, '1985-01-30'	, N'Отдел 2'		, N'Позиция 1'	, 1				, 'Q4wfgMV2qspmiSsmE2zXbg=='	, 'uJPB4aEqMsi8tZKvgja4nlBvg51wwjgCrAGDQRhoTKheuSg3aZ0+DWugtSevQW848HQpD5kh1LGK0KwIM836v6Z5Bif39lFPQd8MSq8dTm+3iLs1/127JZIsO6G0wfodozxb7TyD18YPrPqcZs4ybFSOf4TrrE07MUGV8fd+Tb4=')
	, ('239B6433-67A0-40C1-9647-B4FF81DD15D1'	, N'Георги'		, N'Георгиев'	, 'georgi@gmail.com'	, 'RO'					, '1902-11-06'	, N'Отдел 1'		, N'Позиция 3'	, 0				, 'u9B9Ssw4uWsS73MmoWpWrA=='	, '7iyCSf23lxLla2aJCFXu6TTub+NPhOTVLLc4+NwWyyw8oh3czMZYXtGsYfbs5FQ9r/94AePR8nDvmM21/9KulXVc4bIQdXYycTTdQmMh614ezK43Elrk9FspOSy3jRy8YgVPoEvH/wfBH+wz4qFrDpjQKr9CQbGgs0zTJ47fdn4=')
	, ('0501A703-17CE-4B4F-B142-5E66CDE9BA8B'	, N'Марин'		, N'Маринов'	, 'marin@gmail.com'		, 'BG'					, '1967-12-07'	, N'Отдел 2'		, N'Позиция 1'	, 0				, 'fxKA/U0+jZds+tZf+G6lhQ=='	, 'hrX+2k0qtHWKVOZ8SieLzkf3oXn951rchyv/h5LfirDiSRs1lWkIsUs0aGhN6lyzrmUyJdC3elvjC7HzRiTT2ileG+iyQ22r7LTbivCAdffvwlLVgOnpkxh9qDUNe20j4fu6U2i+KAy/J/2YgOT4nYbMoTXqSfktV2EGbH6zxjo=')
	, ('6587B15C-356E-41CF-BA61-9336F80D62D8'	, N'Красимир'	, N'Красимиров'	, 'krasimir@gmail.com'	, 'DE'					, '1977-11-14'	, N'Отдел 2'		, N'Позиция 2'	, 1				, 'FxKAtWVMUVa49efydt9sXw=='	, 'JRaMfH1wVdaxXgMs5kmhgI8+xLrWa393Do0FUIABWiMmvNfTCeSsjGvfxC0ckmy56RAM5+nxIbsg+2u4ZqpNMSw1KGiK8zLcHjAlrCepEpeux44veH3BjQx143f5VjDpNr0FkwdD87/HWEmgYdk/CrG5KOGdRut01hAepAKSFL8=')
GO
