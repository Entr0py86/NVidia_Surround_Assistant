#ifndef _DEFINES_H
#define _DEFINES_H

#pragma once
#include <windows.h>

#define MIN_ALL        419
#define MIN_ALL_UNDO   416

typedef enum _NVAPI_DataType : unsigned int
{
	pathInfo,
	gridTopo,
	combined,
}NVAPI_DataType;

typedef struct _DisplayManager_Header
{
	NVAPI_DataType	fileType;
	unsigned int	size;
}DisplayManager_Header;

typedef struct _DisplayManager_PathInfo
{
	DisplayManager_Header header;
	unsigned int pathCount;
}DisplayManager_PathInfo;

typedef struct _DisplayManager_GridTopo
{
	DisplayManager_Header header;
	unsigned int gridCount;
}DisplayManager_GridTopo;

typedef struct _WindowPos
{
	HWND hWnd;
	WINDOWPLACEMENT winPlacement;
	WCHAR windowTitle[1024];
}WindowPos;
#endif