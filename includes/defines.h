#ifndef _DEFINES_H
#define _DEFINES_H

#pragma once
#include <windows.h>

#define MIN_ALL        419
#define MIN_ALL_UNDO   416

typedef enum _NVAPI_FileType : unsigned int
{
	pathInfo,
	gridTopo,
	combined,
}NVAPI_FileType;

typedef struct _DisplayManager_FileHeader
{
	NVAPI_FileType	fileType;
	unsigned int	size;
}DisplayManager_FileHeader;

typedef struct _DisplayManager_File_PathInfo
{
	DisplayManager_FileHeader header;
	unsigned int pathCount;
}DisplayManager_File_PathInfo;

typedef struct _DisplayManager_File_GridTopo
{
	DisplayManager_FileHeader header;
	unsigned int gridCount;
}DisplayManager_File_GridTopo;

typedef struct _WindowPos
{
	HWND hWnd;
	WINDOWPLACEMENT winPlacement;
	WCHAR windowTitle[1024];
}WindowPos;
#endif