// rgc-bot.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "rgc-bot.h"
#include "RgcInterface.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

IRgcInterface* CreateInterface()
{
	IRgcInterface* iface = (IRgcInterface*) new RgcInterface();

	return iface;
}

void DeleteInterface(IRgcInterface* iface)
{
	if (iface)
	{
		delete iface;
	}
}