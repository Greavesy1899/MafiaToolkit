#ifndef M2_MODIFIERS_HEADERS
#define M2_MODIFIERS_HEADERS
#include "3dsmaxsdk_preinclude.h"
#include "resource.h"
#include "M2Plugin.h"
#include <simpmod.h>
#include "dummy.h"

#define M2_MODIFIER_CLASS_ID	Class_ID(0x140977, 0x66aa48a8)

class M2Modifier : public Modifier
{
public:
	//Constructor/Destructor
	M2Modifier();
	virtual ~M2Modifier();

	virtual void DeleteThis() { delete this; }

	// From Animatable
	virtual const TCHAR *GetObjectName() { return GetString(IDS_M2_MODIFIER); }

	virtual ChannelMask ChannelsUsed() { return GEOM_CHANNEL | TOPO_CHANNEL; }
#pragma message(TODO("Add the channels that the modifier actually modifies"))
	virtual ChannelMask ChannelsChanged() { return GEOM_CHANNEL; }

	Class_ID InputType() { return defObjectClassID; }

	virtual void ModifyObject(TimeValue t, ModContext &mc, ObjectState *os, INode *node);
	virtual void NotifyInputChanged(const Interval& changeInt, PartID partID, RefMessage message, ModContext *mc);

	virtual void NotifyPreCollapse(INode *node, IDerivedObject *derObj, int index);
	virtual void NotifyPostCollapse(INode *node, Object *obj, IDerivedObject *derObj, int index);


	virtual Interval LocalValidity(TimeValue t);

	// From BaseObject
#pragma message(TODO("Return true if the modifier changes topology"))
	virtual BOOL ChangeTopology() { return FALSE; }

	virtual CreateMouseCallBack* GetCreateMouseCallBack() { return NULL; }

	virtual BOOL HasUVW();
	virtual void SetGenUVW(BOOL sw);


	virtual void BeginEditParams(IObjParam *ip, ULONG flags, Animatable *prev);
	virtual void EndEditParams(IObjParam *ip, ULONG flags, Animatable *next);


	virtual Interval GetValidity(TimeValue t);

	// Automatic texture support

	// Loading/Saving
	virtual IOResult Load(ILoad *iload);
	virtual IOResult Save(ISave *isave);

	//From Animatable
	virtual Class_ID ClassID() { return M2_MODIFIER_CLASS_ID; }
	virtual SClass_ID SuperClassID() { return OSM_CLASS_ID; }
	virtual void GetClassName(TSTR& s) { s = GetString(IDS_M2_MODIFIER); }

	virtual RefTargetHandle Clone(RemapDir &remap);
	virtual RefResult NotifyRefChanged(const Interval& changeInt, RefTargetHandle hTarget, PartID& partID, RefMessage message, BOOL propagate);

	virtual int NumSubs() { return 1; }
	virtual TSTR SubAnimName(int /*i*/) { return GetString(IDS_PARAMS); }
	virtual Animatable* SubAnim(int /*i*/) { return pblock; }

	// TODO: Maintain the number or references here
	virtual int NumRefs() { return 1; }
	virtual RefTargetHandle GetReference(int i);

	virtual int	NumParamBlocks() { return 1; }					// return number of ParamBlocks in this instance
	virtual IParamBlock2* GetParamBlock(int /*i*/) { return pblock; } // return i'th ParamBlock
	virtual IParamBlock2* GetParamBlockByID(BlockID id) { return (pblock->ID() == id) ? pblock : NULL; } // return id'd ParamBlock

protected:
	virtual void SetReference(int, RefTargetHandle rtarg);

private:
	// Parameter block
	IParamBlock2 * pblock; //ref 0
};


class M2ModifierMainDlgProc : public ParamMap2UserDlgProc {
public:
	M2Modifier * mod;
	M2ModifierMainDlgProc() { mod = NULL; }
	void SetEnables(HWND hWnd);
	void UpdateSelLevelDisplay(HWND hWnd);
	INT_PTR DlgProc(TimeValue t, IParamMap2 *map, HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
	void DeleteThis() { }
};

#endif
