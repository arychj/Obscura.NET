﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Obscura.Common
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Obscura")]
	public partial class ObscuraLinqDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ObscuraLinqDataContext() : 
				base(global::Obscura.Properties.Settings.Default.ObscuraConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ObscuraLinqDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObscuraLinqDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObscuraLinqDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObscuraLinqDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspLogHit")]
		public int xspLogHit([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateEntity")]
		public int xspUpdateEntity([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> typeid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string title, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string description, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> active, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, typeid, type, title, description, active, resultcode);
			entityid = ((System.Nullable<int>)(result.GetParameterValue(0)));
			typeid = ((System.Nullable<int>)(result.GetParameterValue(1)));
			resultcode = ((string)(result.GetParameterValue(6)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdatePhoto")]
		public ISingleResult<xspUpdatePhotoResult> xspUpdatePhoto([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> thumbnailid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> imageid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, thumbnailid, imageid, resultcode);
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdatePhotoResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetPhoto")]
		public ISingleResult<xspGetPhotoResult> xspGetPhoto([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> thumbnailid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> imageid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, thumbnailid, imageid, resultcode);
			entityid = ((System.Nullable<int>)(result.GetParameterValue(0)));
			thumbnailid = ((System.Nullable<int>)(result.GetParameterValue(1)));
			imageid = ((System.Nullable<int>)(result.GetParameterValue(2)));
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspGetPhotoResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetSetting")]
		public int xspGetSetting([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(100)")] ref string name, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(MAX)")] ref string value, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] ref System.Nullable<bool> tfEncrypted, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, name, value, tfEncrypted, resultcode);
			id = ((System.Nullable<int>)(result.GetParameterValue(0)));
			name = ((string)(result.GetParameterValue(1)));
			value = ((string)(result.GetParameterValue(2)));
			tfEncrypted = ((System.Nullable<bool>)(result.GetParameterValue(3)));
			resultcode = ((string)(result.GetParameterValue(4)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateSetting")]
		public int xspUpdateSetting([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(100)")] string name, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(MAX)")] string value, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> tfEncrypted, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, name, value, tfEncrypted, resultcode);
			id = ((System.Nullable<int>)(result.GetParameterValue(0)));
			resultcode = ((string)(result.GetParameterValue(4)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetImage")]
		public int xspGetImage([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] ref string path, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] ref string mimetype, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> resolutionX, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> resolutionY, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, path, mimetype, resolutionX, resolutionY, resultcode);
			path = ((string)(result.GetParameterValue(1)));
			mimetype = ((string)(result.GetParameterValue(2)));
			resolutionX = ((System.Nullable<int>)(result.GetParameterValue(3)));
			resolutionY = ((System.Nullable<int>)(result.GetParameterValue(4)));
			resultcode = ((string)(result.GetParameterValue(5)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateImage")]
		public ISingleResult<xspUpdateImageResult> xspUpdateImage([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string path, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string mimetype, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> resolutionX, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> resolutionY, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, path, mimetype, resolutionX, resolutionY, resultcode);
			resultcode = ((string)(result.GetParameterValue(5)));
			return ((ISingleResult<xspUpdateImageResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspDeleteEntity")]
		public int xspDeleteEntity([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, resultcode);
			resultcode = ((string)(result.GetParameterValue(1)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateImageExifData")]
		public ISingleResult<xspUpdateImageExifDataResult> xspUpdateImageExifData([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string value, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, type, value, resultcode);
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdateImageExifDataResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetImageExifData")]
		public ISingleResult<xspGetImageExifDataResult> xspGetImageExifData([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, resultcode);
			resultcode = ((string)(result.GetParameterValue(1)));
			return ((ISingleResult<xspGetImageExifDataResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetEntityMembers")]
		public ISingleResult<xspGetEntityMembersResult> xspGetEntityMembers([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, resultcode);
			resultcode = ((string)(result.GetParameterValue(1)));
			return ((ISingleResult<xspGetEntityMembersResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspDeleteEntityMember")]
		public int xspDeleteEntityMember([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> memberid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, memberid, resultcode);
			resultcode = ((string)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateEntityMember")]
		public int xspUpdateEntityMember([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> memberid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, entityid, memberid, resultcode);
			id = ((System.Nullable<int>)(result.GetParameterValue(0)));
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateAlbum")]
		public ISingleResult<xspUpdateAlbumResult> xspUpdateAlbum([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> thumbnailid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, thumbnailid, resultcode);
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdateAlbumResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetCollection")]
		public int xspGetCollection([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> thumbid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, thumbid, resultcode);
			coverid = ((System.Nullable<int>)(result.GetParameterValue(1)));
			thumbid = ((System.Nullable<int>)(result.GetParameterValue(2)));
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateCollection")]
		public ISingleResult<xspUpdateCollectionResult> xspUpdateCollection([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> thumbnailid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, thumbnailid, resultcode);
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdateCollectionResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetAlbum")]
		public int xspGetAlbum([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> thumbid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, thumbid, resultcode);
			coverid = ((System.Nullable<int>)(result.GetParameterValue(1)));
			thumbid = ((System.Nullable<int>)(result.GetParameterValue(2)));
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetJournal")]
		public void xspGetJournal([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Text")] ref string body, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, body, resultcode);
			coverid = ((System.Nullable<int>)(result.GetParameterValue(1)));
			body = ((string)(result.GetParameterValue(2)));
			resultcode = ((string)(result.GetParameterValue(3)));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateJournal")]
		public ISingleResult<xspUpdateJournalResult> xspUpdateJournal([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> coverid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Text")] string body, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, coverid, body, resultcode);
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdateJournalResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetEntityTags")]
		public ISingleResult<xspGetEntityTagsResult> xspGetEntityTags([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, resultcode);
			resultcode = ((string)(result.GetParameterValue(1)));
			return ((ISingleResult<xspGetEntityTagsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspUpdateEntityTag")]
		public ISingleResult<xspUpdateEntityTagResult> xspUpdateEntityTag([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string tag, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, entityid, tag, resultcode);
			id = ((System.Nullable<int>)(result.GetParameterValue(0)));
			resultcode = ((string)(result.GetParameterValue(3)));
			return ((ISingleResult<xspUpdateEntityTagResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspDeleteEntityTag")]
		public ISingleResult<xspDeleteEntityTagResult> xspDeleteEntityTag([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string tag, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, tag, resultcode);
			resultcode = ((string)(result.GetParameterValue(2)));
			return ((ISingleResult<xspDeleteEntityTagResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetEntity")]
		public ISingleResult<xspGetEntityResult> xspGetEntity([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> entityid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TypeId", DbType="Int")] ref System.Nullable<int> typeId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Type", DbType="VarChar(50)")] ref string type, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Title", DbType="VarChar(50)")] ref string title, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Description", DbType="VarChar(1000)")] ref string description, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Hits", DbType="Int")] ref System.Nullable<int> hits, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="CreatedOn", DbType="DateTime")] ref System.Nullable<System.DateTime> createdOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ModifiedOn", DbType="DateTime")] ref System.Nullable<System.DateTime> modifiedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsActive", DbType="Bit")] ref System.Nullable<bool> isActive, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), entityid, typeId, type, title, description, hits, createdOn, modifiedOn, isActive, resultcode);
			typeId = ((System.Nullable<int>)(result.GetParameterValue(1)));
			type = ((string)(result.GetParameterValue(2)));
			title = ((string)(result.GetParameterValue(3)));
			description = ((string)(result.GetParameterValue(4)));
			hits = ((System.Nullable<int>)(result.GetParameterValue(5)));
			createdOn = ((System.Nullable<System.DateTime>)(result.GetParameterValue(6)));
			modifiedOn = ((System.Nullable<System.DateTime>)(result.GetParameterValue(7)));
			isActive = ((System.Nullable<bool>)(result.GetParameterValue(8)));
			resultcode = ((string)(result.GetParameterValue(9)));
			return ((ISingleResult<xspGetEntityResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspWriteException")]
		public int xspWriteException([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] ref System.Nullable<int> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string origin, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string message, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(MAX)")] string details, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Text")] string stacktrace, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(1000)")] string url, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(15)")] string clientip, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, origin, type, message, details, stacktrace, url, clientip, resultcode);
			id = ((System.Nullable<int>)(result.GetParameterValue(0)));
			resultcode = ((string)(result.GetParameterValue(8)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.xspGetEntities")]
		public ISingleResult<xspGetEntitiesResult> xspGetEntities([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(10)")] ref string resultcode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), type, resultcode);
			resultcode = ((string)(result.GetParameterValue(1)));
			return ((ISingleResult<xspGetEntitiesResult>)(result.ReturnValue));
		}
	}
	
	public partial class xspUpdatePhotoResult
	{
		
		private int _id;
		
		public xspUpdatePhotoResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspGetPhotoResult
	{
		
		private System.Nullable<int> _id_entity;
		
		private string _path;
		
		private System.Nullable<int> _resolutionX;
		
		private System.Nullable<int> _resolutionY;
		
		public xspGetPhotoResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id_entity", DbType="Int")]
		public System.Nullable<int> id_entity
		{
			get
			{
				return this._id_entity;
			}
			set
			{
				if ((this._id_entity != value))
				{
					this._id_entity = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_path", DbType="VarChar(1000)")]
		public string path
		{
			get
			{
				return this._path;
			}
			set
			{
				if ((this._path != value))
				{
					this._path = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_resolutionX", DbType="Int")]
		public System.Nullable<int> resolutionX
		{
			get
			{
				return this._resolutionX;
			}
			set
			{
				if ((this._resolutionX != value))
				{
					this._resolutionX = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_resolutionY", DbType="Int")]
		public System.Nullable<int> resolutionY
		{
			get
			{
				return this._resolutionY;
			}
			set
			{
				if ((this._resolutionY != value))
				{
					this._resolutionY = value;
				}
			}
		}
	}
	
	public partial class xspUpdateImageResult
	{
		
		private int _id;
		
		public xspUpdateImageResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspUpdateImageExifDataResult
	{
		
		private int _id;
		
		public xspUpdateImageExifDataResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspGetImageExifDataResult
	{
		
		private string _Type;
		
		private string _Value;
		
		public xspGetImageExifDataResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(50)")]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this._Type = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Value", DbType="VarChar(50)")]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if ((this._Value != value))
				{
					this._Value = value;
				}
			}
		}
	}
	
	public partial class xspGetEntityMembersResult
	{
		
		private int _id_member;
		
		public xspGetEntityMembersResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id_member", DbType="Int NOT NULL")]
		public int id_member
		{
			get
			{
				return this._id_member;
			}
			set
			{
				if ((this._id_member != value))
				{
					this._id_member = value;
				}
			}
		}
	}
	
	public partial class xspUpdateAlbumResult
	{
		
		private int _id;
		
		public xspUpdateAlbumResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspUpdateCollectionResult
	{
		
		private int _id;
		
		public xspUpdateCollectionResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspUpdateJournalResult
	{
		
		private int _id;
		
		public xspUpdateJournalResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspGetEntityTagsResult
	{
		
		private int _EntityId;
		
		private int _TagId;
		
		private string _Tag;
		
		public xspGetEntityTagsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EntityId", DbType="Int NOT NULL")]
		public int EntityId
		{
			get
			{
				return this._EntityId;
			}
			set
			{
				if ((this._EntityId != value))
				{
					this._EntityId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TagId", DbType="Int NOT NULL")]
		public int TagId
		{
			get
			{
				return this._TagId;
			}
			set
			{
				if ((this._TagId != value))
				{
					this._TagId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Tag", DbType="VarChar(50)")]
		public string Tag
		{
			get
			{
				return this._Tag;
			}
			set
			{
				if ((this._Tag != value))
				{
					this._Tag = value;
				}
			}
		}
	}
	
	public partial class xspUpdateEntityTagResult
	{
		
		private int _id;
		
		public xspUpdateEntityTagResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspDeleteEntityTagResult
	{
		
		private int _id;
		
		public xspDeleteEntityTagResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
	
	public partial class xspGetEntityResult
	{
		
		private string _Tag;
		
		public xspGetEntityResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Tag", DbType="VarChar(50)")]
		public string Tag
		{
			get
			{
				return this._Tag;
			}
			set
			{
				if ((this._Tag != value))
				{
					this._Tag = value;
				}
			}
		}
	}
	
	public partial class xspGetEntitiesResult
	{
		
		private int _id;
		
		public xspGetEntitiesResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL")]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
