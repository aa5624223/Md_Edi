var datatable;
var DataList;
var EditFlg = false;
var hosturl = window.location.protocol + "//" + window.location.host;
var v_DiaLog_add;
var v_DiaLog_edit;
var v_dialog_search;
var dialog_add;
var dialog_edit;
var dialog_search;
var Vue_Search;
var Vue_add;
var Vue_edit;
$(document).ready(function () {
    $("input[type=file]").change(function () {
        var fieldVal = $(this).val();
        // Change the node's value by removing the fake path (Chrome)
        fieldVal = fieldVal.replace("C:\\fakepath\\", "");

        if (fieldVal != undefined || fieldVal != "") {
            $(this).next(".custom-file-label").attr('data-content', fieldVal);
            $(this).next(".custom-file-label").text(fieldVal);
        }
    });
    /* 对话框初始化 */
    dialog_add = $("#dialog_add");
    dialog_edit = $("#dialog_edit");
    dialog_search = $("#dialog_search");
    /* 表格初始化  */
    var LanguagePage = {
        "sProcessing": "数据获取中",
        "sLengthMenu": "显示 _MENU_ 条",
        "sZeroRecords": "没有您要搜索的内容",
        "sInfo": "从 _START_ 到  _END_ 条记录 总记录 _TOTAL_ 条",
        "sInfoEmpty": "记录数为0",
        "sInfoFiltered": "(全部记录 _MAX_ 条)",
        "sInfoPostFix": "",
        "sSearch": "搜索：",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "第一页",
            "sPrevious": "上一页",
            "sNext": "下一页",
            "sLast": "最后一页"
        },
        buttons: {
            copyTitle: '表格已复制',
            copySuccess: {
                _: '%d 行数据已复制',
                1: '1 行数据已复制'
            }
        }
    };
    datatable = $("#tab_dataInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        bAutoWidth: true,
        height: 300,
        scrollX: true,
        sScrollY: 520, //DataTables的高 
        bFilter: false,
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_HC_EDI_WLDZ',
            type: 'POST',
            'dataSrc': function (json) {
                if (json.msg == "refuse") {
                    Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                    return [];
                } else {
                    DataList = isJsonString(json.data);
                    return DataList;
                }
                
            }
        },
        columns: [
            {//全选 0
                "data": null,
                "defaultContent": "",
                "searchable": false,
                "orderable": false,
                "visible": true,
                "width": "10px",
                "className": "td-oprate",
                render: function (data, index1, index2, index3) {
                    var idx = index3.row;
                    // 741
                    return '<input type="checkbox" name="TabCheck1" onclick="SingleCheck(this)" />'
                }
            },
            {// 供方物料编码  1
                "data": "supplyItemCode",
                "defaultContent": '',
                "orderable": true,
                "searchable": true,
            },
            {// 供方物料描述  2
                "data": "itemDescription",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                visible: false
            },
            {// 项目编号 3
                "data": "Project",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {//项目名 4
                "data": "ProjectName",
                sWidth: "16%",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {//需求方物料编码 5
                "data": "demandItemCode",
                "defaultContent": '',
                "orderable": true,
                "searchable": true,

            },
            {//需求方物料描述 6
                "data": "demandItemName",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                visible: false
            },
            {//库存组织编码 7
                "data": "organizationCode",
                "defaultContent": '',
                "orderable": true,
                "searchable": true,
            },
            {//生产地 8
                "data": "organizationName",
                "defaultContent": '',
                "orderable": true,
                "searchable": true,
            },
            //{//更新时间 9
            //	"data": "StarDATE",
            //	"defaultContent": '',
            //	"orderable": true,
            //	"searchable": true,
            //},
            {//供货状态 9
                "data": "flag",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    if (data == 1) {
                        return "在供"
                    } else if (data == 2) {
                        return "<font style='color:red,font-weight:600'>" + 停供 + "</font>";
                    } else if (data == -1) {
                        return "<font style='color:red,font-weight:600'>" + 删除 + "</font>";
                    }
                }
            },
            {//备注 10
                "data": "reference",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                visible: false
            },
            {//结存数量 11
                "data": "View.结存数量",
                "defaultContent": '',
                "orderable": false,

                "searchable": false,
            },
            {//确认数量 12
                "data": "QTY",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return '<input type="number" value="' + data + '" disabled style="width:60px;" /> ';
                }
            },
            {//操作 13
                "data": null,
                "defaultContent": '',
                "className": "td-oprate",
                "orderable": false,
                "searchable": false
            }
        ],
        columnDefs: [
            {
                "targets": 13,
                "render": function (data, type, row) {
                    return "<button type='button' onclick='Dialog_Edit(" + JSON.stringify(row) + ")' style='margin-left:10px;font-size:80%' class='btn btn-success btn-sm'>编辑</button>";
                }
            }
        ],
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            api.column(2, { page: 'current' }).data().each(function (group, i) {
                var Type = api.column(6, { page: 'current' }).data()[i];
                $(rows).eq(i).before("" +
                    "<tr class='group'>" +
                    "<td colspan=1></td><td colspan=3>供方描述：" + group + "</td>" +
                    "<td colspan=5>美的描述：" + Type + "</td>" +
                    "<td colspan=2></td>" +
                    "</tr>")
            });
        },
        dom: "Blfrtip",
        buttons: [
            {
                extend: 'copy',
                text: '复制表格',// 显示文字
                exportOptions: {
                    columns: ':not(.td-oprate)'
                }
            },
            {
                extend: 'excelHtml5',
                autoFilter: true,
                messageTop: '美的EDI数据',
                title: '美的EDI数据',
                text: '导出本页',// 显示文字
                exportOptions: {
                    columns: ':not(.td-oprate)',
                    format: {
                        body: function (data, row, column, node) {
                            if (column == 11) {
                                return $(data).val();
                            } else {
                                return data;
                            }
                        }
                    }
                },
                customize: function (xlsx) {
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    $('c[r=L1] t', sheet).text('自定义库存');
                },
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $('td:eq(1)', nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
            },
        ]
    });
    //datatable.on('order.dt search.dt', function () {
    //	datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //		cell.innerHTML = i + 1;
    //	});
    //}).draw();
    //$('#tab_dataInfo tbody').on('mouseenter', 'td', function () {
    //	if (datatable.cell(this).index() != undefined) {
    //		var colIdx = datatable.cell(this).index().column;
    //		$(datatable.cells().nodes()).removeClass('highlight');
    //		$(datatable.column(colIdx).nodes()).addClass('highlight');
    //	}
    //});
    
    
    /* Vue初始化 */
    Vue_Search = new Vue({
        el: '#table_Search',
        data: {
            supplyItemCode: '',
            itemDescription: '',
            demandItemCode: '',
            demandItemName: '',
            Project: '',
            ProjectName: '',
            organizationCode: '',
            organizationName: '',
            flg: '',
        },
        watch: {
            Project: function () {
                datatable.draw();
            },
            ProjectName: function () {
                datatable.draw();
            },
            organizationCode: function () {
                datatable.draw();
            },
            organizationName: function () {
                datatable.draw();
            },
            flg: function () {
                datatable.draw();
            },
            supplyItemCode: function () {
                datatable.draw();
            },
            itemDescription: function () {
                datatable.draw();
            },
            demandItemCode: function () {
                datatable.draw();
            },
            demandItemName: function () {
                datatable.draw();
            }
        }
    })

})
$.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
    var Search_supplyItemCode = Vue_Search._data.supplyItemCode;
    var Search_itemDescription = Vue_Search._data.itemDescription;
    var Search_demandItemCode = Vue_Search._data.demandItemCode;
    var Search_demandItemName = Vue_Search._data.demandItemName;

    var Search_Project = Vue_Search._data.Project;
    var Search_ProjectName = Vue_Search._data.ProjectName;
    var Search_organizationCode = Vue_Search._data.organizationCode;
    var Search_organizationName = Vue_Search._data.organizationName;
    var Search_flg = Vue_Search._data.flg;
    var Search_flg2 = $("#Search_flg2").prop('checked');

    var supplyItemCode = datatable.data()[dataIndex].supplyItemCode;
    var itemDescription = datatable.data()[dataIndex].itemDescription;
    var demandItemCode = datatable.data()[dataIndex].demandItemCode;
    var demandItemName = datatable.data()[dataIndex].demandItemName;

    var Project = datatable.data()[dataIndex].Project;
    var ProjectName = datatable.data()[dataIndex].ProjectName;
    var organizationCode = datatable.data()[dataIndex].organizationCode;
    var organizationName = datatable.data()[dataIndex].organizationName;
    var flag = datatable.data()[dataIndex].flag;
    if (flag == -1) {
        var tt = 0;
    }
    var flg2 = true;

    if (supplyItemCode != Search_supplyItemCode && Search_supplyItemCode != "") {
        flg2 = false;
    }
    if (itemDescription != Search_itemDescription && Search_itemDescription != "") {
        flg2 = false;
    }
    if (demandItemCode != Search_demandItemCode && Search_demandItemCode != "") {
        flg2 = false;
    }
    if (demandItemName != Search_demandItemName && Search_demandItemName != "") {
        flg2 = false;
    }

    if (Project != Search_Project && Search_Project != "") {
        flg2 = false;
    }

    if (Project != Search_Project && Search_Project != "") {
        flg2 = false;
    }
    if (ProjectName.indexOf(Search_ProjectName) == -1 && Search_ProjectName != "") {
        flg2 = false;
    }
    if (organizationCode != Search_organizationCode && Search_organizationCode != "") {
        flg2 = false;
    }
    if (organizationName.indexOf(Search_organizationName) == -1 && Search_organizationName != "") {
        flg2 = false;
    }
    if (flag != Search_flg && Search_flg != "") {
        flg2 = false;
        if (flag == -1 && Search_flg2 == true) {
            flg2 = true;
        }
    }
    return flg2;
})
/* 对话框 */
//添加数据
var add_flg = false;
function Dialog_Add() {
    dialog_add.css("display", "block");
    v_DiaLog_add = $.dialog({
        title: '数据添加',
        closeIcon: true,
        columnClass: 'col-lg-12',
        content: dialog_add,
        onOpen: function () {
            Vue_add = new Vue({
                el: '#dialog_add',
                data: {
                    supplyItemCode: '',
                    itemDescription: '',
                    Project: '',
                    ProjectName: '',
                    demandItemCode: '',
                    demandItemName: '',
                    organizationCode: '',
                    organizationName: '合肥华凌股份-冷柜工厂-制造',
                    reference: '',
                    QTY: 0,
                    flag: 1,
                    add_DataList: [],
                },
                methods: {
                    btn_Submit: function () {
                        var formData = new FormData();
                        var _this = this;
                        formData.append("supplyItemCode", this.supplyItemCode);
                        formData.append("Project", this.Project);
                        formData.append("demandItemCode", this.demandItemCode);
                        formData.append("organizationCode", this.organizationCode);
                        $.ajax({
                            type: 'POST',
                            url: hosturl + '/Home/check_HC_EDI_WLDZ',
                            data: formData,
                            dataType: 'JSON',
                            cache: false,
                            processData: false,
                            contentType: false,
                            success: function (res) {
                                if (res.msg == "OK") {
                                    //Alert1("提示", "添加成功", "btn-primary");
                                    var flg = false;
                                    _this.add_DataList.forEach(function (item) {
                                        if (item.supplyItemCode == _this.supplyItemCode && item.Project == _this.Project && item.demandItemCode == _this.demandItemCode && item.organizationCode == _this.organizationCode) {
                                            flg = true;
                                        }
                                    })
                                    if (flg) {
                                        Alert1("提示", "已添加相同记录，无法添加", "btn-danger", "red");
                                        return;
                                    }
                                    var obj = {};
                                    obj.supplyItemCode = _this.supplyItemCode;
                                    obj.itemDescription = _this.itemDescription;
                                    obj.Project = _this.Project;
                                    obj.ProjectName = _this.ProjectName;
                                    obj.demandItemCode = _this.demandItemCode;
                                    obj.demandItemName = _this.demandItemName;
                                    obj.organizationCode = _this.organizationCode;
                                    obj.organizationName = _this.organizationName;
                                    obj.reference = _this.reference;
                                    obj.flag = parseInt(_this.flag);
                                    obj.QTY = parseFloat(_this.QTY);
                                    _this.add_DataList.push(obj);
                                    _this.Rset();
                                } else if (res.msg == "NOT") {
                                    Alert1("提示", "已存在相同记录，无法添加", "btn-danger", "red");
                                } else if (res.msg == "refuse") {
                                    Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                                }
                            },
                            error: function () {

                            }
                        });

                    },
                    btn_Reset: function () {
                        this.Rset();
                    },
                    btn_Save: function () {
                        if (add_flg) {
                            return;
                        }
                        var _this = this;
                        var formData = new FormData();
                        formData.append("DataList", JSON.stringify(this.add_DataList));
                        $.ajax({
                            type: 'POST',
                            url: hosturl + '/Home/add_HC_EDI_WLDZ',
                            data: formData,
                            dataType: 'JSON',
                            cache: false,
                            processData: false,
                            contentType: false,
                            beforeSend: function () {
                                add_flg = true;
                            },
                            complete: function () {
                                add_flg = false;
                            },
                            success: function (res) {
                                if (res.msg == "OK") {
                                    Alert1("提示", "添加成功", "btn-primary", "green");
                                    _this.add_DataList = [];
                                    datatable.ajax.reload();
                                } else if (res.msg == "error") {
                                    Alert1("提示", "登录超时", "btn-danger", "red");
                                } else if (res.msg == "refuse") {
                                    Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                                }
                            },
                            error: function () {
                                Alert1("提示", "服务器无响应", "btn-danger", "red");
                            }
                        });

                    },
                    Rset: function () {
                        this.supplyItemCode = '';
                        this.itemDescription = '';
                        this.Project = '';
                        this.ProjectName = '';
                        this.demandItemCode = '';
                        this.demandItemName = '';
                        this.organizationCode = '';
                        this.organizationName = '';
                        this.reference = '';
                        this.QTY = 0;
                        this.flag = 1;
                    }
                },
                mounted: function () {

                }
            })

        }
    })

}
//编辑数据
function Dialog_Edit(obj) {
    if (obj.flag == -1 || obj.flag == 2) {
        //停供或删除的数据无法编辑
        Alert1("提示", "停供或删除的数据无法编辑", "btn-danger", "red");
        return;
    }
    dialog_edit.css("display", "block");
    v_DiaLog_edit = $.confirm({
        title: '编辑数据',
        closeIcon: true,
        columnClass: 'col-lg-12',
        content: dialog_edit,
        buttons: {
            add: {
                text: '保存',
                btnClass: 'btn btn-success',
                action: function () {
                    Data_Edit();
                }
            },
            close: {
                text: '关闭'
            }
        },
        onOpen: function () {

            Vue_edit = new Vue({
                el: '#dialog_edit',
                data: {
                    supplyItemCode: '',
                    itemDescription: '',
                    Project: '',
                    ProjectName: '',
                    demandItemCode: '',
                    demandItemName: '',
                    organizationCode: '',
                    organizationName: '',
                    reference: '',
                    QTY: 0,
                    flag: 1,
                    supplyItemCode_id: '',
                    Project_id: '',
                    demandItemCode_id: '',
                    organizationCode_id: ''
                },
                mounted: function () {
                    this.supplyItemCode = obj.supplyItemCode;
                    this.itemDescription = obj.itemDescription;
                    this.Project = obj.Project;
                    this.ProjectName = obj.ProjectName;
                    this.demandItemCode = obj.demandItemCode;
                    this.demandItemName = obj.demandItemName;
                    this.organizationCode = obj.organizationCode;
                    this.organizationName = obj.organizationName;
                    this.reference = obj.reference;
                    this.QTY = parseFloat(obj.QTY);
                    this.flag = parseInt(obj.flag);

                    this.supplyItemCode_id = obj.supplyItemCode;
                    this.Project_id = obj.Project;
                    this.demandItemCode_id = obj.demandItemCode;
                    this.organizationCode_id = obj.organizationCode;
                }
            })
        }
    })
}
//数据查询
function Dialog_Search() {
    dialog_search.css("display", "block");
    v_dialog_search = $.confirm({
        title: '查询',
        closeIcon: true,
        columnClass: 'col-lg-8',
        content: dialog_search,
        buttons: {
            Search: {
                text: '查询',
                btnClass: 'btn btn-success',
                action: function () {
                    //查询
                    datatable.draw();
                }
            },
            close: {
                text: '关闭'
            }
        }
    })

}

/* 添加数据 */
function Add_Data() {

}

/* 编辑数据 */
//编辑单条数据
var edit_flg = false;
function Data_Edit() {
    if (edit_flg) {
        return;
    }

    var formData = new FormData();
    formData.append("supplyItemCode", Vue_edit._data.supplyItemCode);
    formData.append("itemDescription", Vue_edit._data.itemDescription);
    formData.append("Project", Vue_edit._data.Project);
    formData.append("ProjectName", Vue_edit._data.ProjectName);
    formData.append("demandItemCode", Vue_edit._data.demandItemCode);
    formData.append("demandItemName", Vue_edit._data.demandItemName);
    formData.append("organizationCode", Vue_edit._data.organizationCode);
    formData.append("organizationName", Vue_edit._data.organizationName);
    formData.append("reference", Vue_edit._data.reference);
    formData.append("QTY", Vue_edit._data.QTY);
    formData.append("flag", Vue_edit._data.flag);

    formData.append("supplyItemCode_id", Vue_edit._data.supplyItemCode_id);
    formData.append("Project_id", Vue_edit._data.Project_id);
    formData.append("demandItemCode_id", Vue_edit._data.demandItemCode_id);
    formData.append("organizationCode_id", Vue_edit._data.organizationCode_id);
    var supplyItemCode = Vue_edit._data.supplyItemCode;
    var Project = Vue_edit._data.Project;
    var demandItemCode = Vue_edit._data.demandItemCode;
    var organizationCode = Vue_edit._data.organizationCode;

    var supplyItemCode_id = Vue_edit._data.supplyItemCode_id;
    var Project_id = Vue_edit._data.Project_id;
    var demandItemCode_id = Vue_edit._data.demandItemCode_id;
    var organizationCode_id = Vue_edit._data.organizationCode_id;
    var canAjax = true;
    if (supplyItemCode != supplyItemCode_id || Project != Project_id || demandItemCode != demandItemCode_id || organizationCode != organizationCode_id) {
        $.ajax({
            type: 'POST',
            url: hosturl + '/Home/check_HC_EDI_WLDZ',
            data: formData,
            dataType: 'JSON',
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.msg == "OK") {//可以添加
                    canAjax = true;
                } else if (res.msg == "NOT") {
                    canAjax = false;
                    Alert1("提示", "已存在相同记录，无法添加", "btn-danger", "red");
                } else if (res.msg == "refuse") {
                    Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                }
            }
        });
    }
    if (canAjax) {
        $.ajax({
            type: 'POST',
            url: hosturl + '/Home/edit_HC_EDI_WLDZ',
            data: formData,
            dataType: 'JSON',
            cache: false,
            processData: false,
            contentType: false,
            beforeSend: function () {
                edit_flg = true;
            },
            complete: function () {
                edit_flg = false;
            },
            success: function (res) {
                if (res.msg == "OK") {
                    Alert1("提示", "修改成功", "btn-success", "green");
                    datatable.ajax.reload();
                } else if (res.msg == "refuse") {
                    Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                } else if (res.msg == "error") {
                    Alert1("提示", "登录超时", "btn-danger", "red");
                }
            },
            error: function () {
                Alert1("提示", "服务器无响应", "btn-danger", "red");
            }
        });
    } else {//不通过 不编辑
        return;
    }
    //验证数据的正确性

}
//编辑多条数据
var Edit_CheckFlg = false;
function Edit_Check() {
    if (Edit_CheckFlg) {
        return;
    }
    var rows = datatable.rows({ page: 'current' }).nodes();
    var EditList = [];
    var CanEdit = true;
    rows.each(function (item, index) {
        var check = $($(item).find("input:checkbox[name=TabCheck1]")[0]);
        if (check.prop("checked") == true) {
            var obj = {};
            obj.QTY = $(this[index]).find("input[type=number]").val();
            obj.supplyItemCode = $(this[index]).children()[1].textContent;//供方编码
            obj.Project = $(this[index]).children()[3].textContent;//项目编号
            obj.demandItemCode = $(this[index]).children()[5].textContent;//需方编码
            obj.organizationCode = $(this[index]).children()[7].textContent;//库存编码
            obj.flag = $(this[index]).children()[9].textContent;//库存编码
            if (obj.flag == "停供" || obj.flag == "删除") {
                CanEdit == false;
            }
            EditList.push(obj);
        }
    })
    if (EditList.length <= 0) {
        Alert1("提示", "请选择对应数据，再点击批量保存修改。", "btn-danger", "red");
        return;
    }
    if (!CanEdit) {
        Alert1("提示", "停供或删除的数据无法编辑。", "btn-danger", "red");
        return;
    }
    var formData = new FormData();
    formData.append("DataList", JSON.stringify(EditList));

    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_Check_HC_EDI_WLDZ',
        data: formData,
        dataType: 'JSON',
        cache: false,
        processData: false,
        contentType: false,
        beforeSend: function () {
            Edit_CheckFlg = true;
        },
        complete: function () {
            Edit_CheckFlg = false;
        },
        success: function (res) {
            if (res.msg == "OK") {
                Alert1("提示", "保存成功", "btn-success", "green");
                datatable.ajax.reload();
            } else if (res.msg == "refuse") {
                Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
            } else if (res.msg == "error") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}

/* 删除数据 */
var Data_Del = false;
function Del_Check() {
    if (Data_Del) {
        return;
    }

    var rows = datatable.rows({ page: 'current' }).nodes();
    var DelList = [];
    rows.each(function (item, index) {
        var check = $($(item).find("input:checkbox[name=TabCheck1]")[0]);
        if (check.prop("checked") == true) {
            var obj = {};
            obj.QTY = $(this[index]).find("input[type=number]").val();
            obj.supplyItemCode = $(this[index]).children()[1].textContent;//供方编码
            obj.Project = $(this[index]).children()[3].textContent;//项目编号
            obj.demandItemCode = $(this[index]).children()[5].textContent;//需方编码
            obj.organizationCode = $(this[index]).children()[7].textContent;//库存编码
            obj.flag = $(this[index]).children()[9].textContent;//库存编码
            DelList.push(obj);
        }
    })
    if (DelList.length <= 0) {
        Alert1("提示", "请选择对应数据，再点击批量删除。", "btn-danger", "red");
        return;
    }
    if (DelList.length != 1) {
        Alert1("提示", "单次只能删除一条数据。", "btn-danger", "red");
        return;
    }
    if (DelList[0].flag == "停供") {
        Alert1("提示", "停供数据无法编辑。", "btn-danger", "red");
        return;
    }
    if (DelList[0].flag == "删除") {
        Alert1("提示", "删除数据无法编辑。", "btn-danger", "red");
        return;
    }

    $.confirm({
        title: '提示',
        content: '确认删除此条数据？',
        buttons: {
            confirm: {
                text: '确定',
                btnClass: 'btn btn-success',
                actions: function () {
                    var formData = new FormData();
                    formData.append("DataList", JSON.stringify(DelList));
                    $.ajax({
                        type: 'POST',
                        url: hosturl + '/Home/Del_Check_HC_EDI_WLDZ',
                        data: formData,
                        dataType: 'JSON',
                        cache: false,
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            Edit_CheckFlg = true;
                        },
                        complete: function () {
                            Edit_CheckFlg = false;
                        },
                        success: function (res) {
                            if (res.msg == "OK") {
                                Alert1("提示", "删除成功", "btn-success", "green");
                                datatable.ajax.reload();
                            } else if (res.msg == "refuse") {
                                Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
                            } else if (res.msg == "error") {
                                Alert1("提示", "登录超时", "btn-danger", "red");
                            }
                        },
                        error: function () {
                            Alert1("提示", "服务器无响应", "btn-danger", "red");
                        }
                    })
                }
            },
            cancel: {
                text: '取消',
                btnClass: 'btn',
            },
        }
    })
}

/* 事件 */
//全选
function CheckAll1(obj) {
    if ($(obj).prop('checked')) {
        //.prop('checked', true)
        $("input:checkbox[name=TabCheck1]").each(function (index, item) {
            $(item).prop("checked", true);
        })
        $("#tab_dataInfo tr").addClass("table-warning");
        $("#tab_dataInfo input[type=number]").removeAttr("disabled");
    } else {
        $("input:checkbox[name=TabCheck1]").each(function (index, item) {
            $(item).prop("checked", false);
        })
        $("#tab_dataInfo tr").removeClass("table-warning");
        $("#tab_dataInfo input[type=number]").attr("disabled", "disabled");
    }
}
//单选事件 
function SingleCheck(obj) {
    var rows = datatable.rows({ page: 'current' }).nodes();
    var tr = $(obj).parent().parent();
    if ($(obj).prop("checked") == false) {
        tr.removeClass("table-warning");
        tr.find('input[type=number]').attr("disabled", "disabled");
    } else {
        tr.addClass("table-warning");
        tr.find('input[type=number]').removeAttr("disabled");
    }

}
//拷贝全部
function CopyAll() {
    var rows = datatable.rows({ page: 'current' }).nodes();
    datatable.column(11, { page: 'current' }).data().each(function (group, i) {
        var check = $(rows).eq(i).find('td').eq(0).children();
        if (check.prop("checked") == true) {
            $(rows).eq(i).find('td').eq(-2).html(
                '<input type="number" value="' + group + '" style="width:60px;" /> '
            )
        }

    })
}
//public methods
function isJsonString(str) {
    try {
        return JSON.parse(str);
    } catch (e) {
        return false;
    }
    return false;
}
function Alert1(title, content, btnClass, type) {
    $.alert({
        backgroundDismiss: true,
        title: title,
        type: type,
        content: content,
        buttons: {
            cancel: {
                text: '确定',
                btnClass: btnClass,
            }
        }
    })
}
function AlertAndReload(title, content, btnClass) {
    $.alert({
        backgroundDismiss: true,
        title: title,
        content: content,
        buttons: {
            cancel: {
                text: '确定',
                btnClass: btnClass,
                actions: function () {
                    window.location.reload();//刷新页面
                }
            }
        }
    })
}

function fix(num, length) {
    return ('' + num).length < length ? ((new Array(length + 1)).join('0') + num).slice(-length) : '' + num;
}
/* 通过excel修改数据 */
function ExcelUpload() {
    if (typeof FileReader == 'undefined') {
        Alert1("提示", "您的浏览器不支持文件读取", "btn-danger", "red");
        return;
    }
    var file = document.getElementById("ExcelFile").files[0];
    if (file == undefined) {
        Alert1("提示", "请选择Excel文件", "btn-danger", "red");
        return;
    }
    var FileSuf = file.name.substring(file.name.lastIndexOf("."), file.name.length);
    if (!(FileSuf == '.xls' || FileSuf == '.xlsx')) {
        Alert1("提示", "只支持xls,xlsx类型的文件", "btn-danger", "red");
        return;
    }
    var reader = new FileReader();
    reader.onload = function (e) {
        SentExcel(e);
    }
    reader.readAsBinaryString(file);
}
var SentExcelFlg = false;
function SentExcel(e) {
    if (SentExcelFlg) {
        return;
    }
    var data = e.target.result;
    var workbook = XLSX.read(data, { type: 'binary' });
    var sheetName = workbook.SheetNames[0];
    var sheet = workbook.Sheets[sheetName];
    var i = 2;//从2开始读取
    while (sheet["A" + i] != undefined) {
        i++;
    }
    //检验表格完整性
    if (!CheckExcel(sheet, i - 1)) {
        return;
    }
    //将sheet 转为对象
    var objs = ExcelToObj(sheet, i - 1);
    var formData = new FormData();
    formData.append("DataList", JSON.stringify(objs))
    if (SentExcelFlg) {
        return;
    }
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_Check_Excel_HC_EDI_WLDZ',
        data: formData,
        dataType: 'JSON',
        cache: false,
        processData: false,
        contentType: false,
        beforeSend: function () {
            SentExcelFlg = true;
        },
        complete: function () {
            SentExcelFlg = false;
        },
        success: function (res) {
            if (res.msg == "OK") {
                var DataList = isJsonString(res.data);
                if (DataList.length == 0) {
                    Alert1("提示", "全部更新成功", "btn-success", "green");
                } else {
                    var div = $("<div><h4>更新失败,下列" + DataList.length + "条数据导致没有更新成功：</h4></div>");
                    var j = 1;
                    DataList.forEach(function (item, index) {
                        var subdiv = $("<div></div>");
                        subdiv.html(j + ".供方编码：" + item.supplyItemCode + "，项目编号：" + item.Project + "，美的编码：" + item.demandItemCode + "，库存编码：" + item.organizationCode + ",自定义库存：" + item.QTY);
                        div.append(subdiv);
                        j++;
                    })
                    div.prepend();
                    $.alert({
                         title: "提示",
                        type: "red",
                        content: div,
                        columnClass: 'col-lg-10',
                        buttons: {
                            cancel: {
                                text: '确定',
                                btnClass: "btn-danger",
                            }
                        }
                    })
                }

                datatable.ajax.reload();
            } else if (res.msg == "refuse") {
                Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
            }else if (res.msg == "error") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            }//要判断一下部分没有修改成功
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    })
}
/* 通过excel添加数据 */
function ExcelUpload2() {
    if (typeof FileReader == 'undefined') {
        Alert1("提示", "您的浏览器不支持文件读取", "btn-danger", "red");
        return;
    }
    var file = document.getElementById("ExcelFile2").files[0];
    if (file == undefined) {
        Alert1("提示", "请选择Excel文件", "btn-danger", "red");
        return;
    }
    var FileSuf = file.name.substring(file.name.lastIndexOf("."), file.name.length);
    if (!(FileSuf == '.xls' || FileSuf == '.xlsx')) {
        Alert1("提示", "只支持xls,xlsx类型的文件", "btn-danger", "red");
        return;
    }
    var reader = new FileReader();
    reader.onload = function (e) {
        SentExcel2(e);
    }
    reader.readAsBinaryString(file);
}


var SentExcelFlg2 = false;
function SentExcel2(e) {
    if (SentExcelFlg2) {
        return;
    }
    var data = e.target.result;
    var workbook = XLSX.read(data, { type: 'binary' });
    var sheetName = workbook.SheetNames[0];
    var sheet = workbook.Sheets[sheetName];
    var i = 2;//从2开始读取
    while (sheet["A" + i] != undefined) {
        i++;
    }
    //检验表格完整性
    if (!CheckExcel2(sheet, i - 1)) {
        return;
    }
    //将sheet 转为对象
    var objs = ExcelToObj2(sheet, i - 1);
    var formData = new FormData();
    formData.append("DataList", JSON.stringify(objs))
    if (SentExcelFlg2) {
        return;
    }
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_Check_Excel_HC_EDI_WLDZ',
        data: formData,
        dataType: 'JSON',
        cache: false,
        processData: false,
        contentType: false,
        beforeSend: function () {
            SentExcelFlg2 = true;
        },
        complete: function () {
            SentExcelFlg2 = false;
        },
        success: function (res) {
            if (res.msg == "OK") {
                Alert1("提示", "全部添加成功", "btn-success", "green");
                datatable.ajax.reload();
            } else if (res.msg == "refuse") {
                Alert1("提示", "登录超时,请刷新页面重新登录", "btn-danger", "red");
            }else if (res.msg == "SQLERROR") {
                var Data = isJsonString(res.data);

                var div = $("<div><h4>添加失败,数据库中已有下列数据无法添加：</h4></div>");
                var j = 1;
                var subdiv = $("<div></div>");
                subdiv.html(j + ".供方编码：" + Data.supplyItemCode + "，项目编号：" + Data.Project + "，美的编码：" + Data.demandItemCode + "，库存编码：" + Data.organizationCode + ",自定义库存：" + Data.QTY);
                div.append(subdiv);
                div.prepend();
                $.alert({
                    backgroundDismiss: true,
                    title: "提示",
                    type: "red",
                    content: div,
                    columnClass: 'col-lg-10',
                    buttons: {
                        cancel: {
                            text: '确定',
                            btnClass: "btn-danger",
                        }
                    }
                })

            } else if (res.msg == "error") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            }//要判断一下部分没有修改成功
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    })
}


function ExcelToObj(sheet, i) {
    var objs = [];
    for (var j = 2; j <= i; j++) {
        var obj = {};
        obj.supplyItemCode = sheet["A" + j].v;

        obj.Project = sheet["B" + j].v;
        obj.demandItemCode = sheet["C" + j].v;
        obj.organizationCode = sheet["D" + j].v;
        obj.QTY = sheet["E" + j].v;
        objs.push(obj);
    }
    return objs;
}


function ExcelToObj2(sheet, i) {
    var objs = [];
    for (var j = 2; j <= i; j++) {
        var obj = {};
        obj.supplyItemCode = sheet["A" + j].v;//主键不为空
        if (sheet["B" + j] == undefined) {
            obj.itemDescription = "";
        } else {
            obj.itemDescription = sheet["B" + j].v;
        }
        obj.Project = sheet["C" + j].v;//主键不为空
        if (sheet["D" + j] == undefined) {
            obj.ProjectName = "";
        } else {
            obj.ProjectName = sheet["D" + j].v;
        }
        obj.demandItemCode = sheet["E" + j].v;//主键不为空
        if (sheet["F" + j] == undefined) {
            obj.demandItemName = "";
        } else {
            obj.demandItemName = sheet["F" + j].v;
        }
        obj.organizationCode = sheet["G" + j].v;//主键不为空
        if (sheet["H" + j] == undefined) {
            obj.organizationName = "";
        } else {
            obj.organizationName = sheet["H" + j].v;
        }
        if (sheet["I" + j] == undefined) {
            obj.flag = 0;
        } else if (sheet["I" + j].v == "停供") {
            obj.flag = 2;
        } else if (sheet["I" + j].v == "在供") {
            obj.flag = 1;
        } else {
            obj.flag = 0;
        }
        if (sheet["J" + j] == undefined) {
            obj.reference = "";
        } else {
            obj.reference = sheet["J" + j].v;
        }
        if (sheet["L" + j] == undefined) {
            obj.QTY = 0;
        } else {
            obj.QTY = sheet["L" + j].v;
        }
        objs.push(obj);
    }
    return objs;
}

function CheckExcel(sheet, i) {
    //检查第一行有无出现断层
    if (sheet["A" + i] == undefined && sheet["A" + (i + 1)] != undefined) {
        Alert1("提示", "A" + i + "没有填写数据,请检查数据完整性", "btn-danger", "red");
        return false;
    }
    //检查B C D E 有无出现断层
    for (var j = 2; j < i; j++) {

        if (sheet["B" + j] == undefined && sheet["B" + (j + 1)] != undefined) {
            Alert1("提示", "B" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["C" + j] == undefined && sheet["C" + (j + 1)] != undefined) {
            Alert1("提示", "C" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["D" + j] == undefined && sheet["D" + (j + 1)] != undefined) {
            Alert1("提示", "D" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["E" + j] == undefined && sheet["E" + (j + 1)] != undefined) {
            Alert1("提示", "E" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
    }
    if (sheet["B" + i] == undefined || sheet["C" + i] == undefined || sheet["D" + i] == undefined || sheet["E" + i] == undefined) {
        Alert1("提示", "请检查第" + i + "行数据的完整性", "btn-danger", "red");
        return false;
    }
    return true;
}

function CheckExcel2(sheet, i) {
    //检查第一行有无出现断层
    if (sheet["A" + i] == undefined && sheet["A" + (i + 1)] != undefined) {
        Alert1("提示", "A" + i + "没有填写数据,请检查数据完整性", "btn-danger", "red");
        return false;
    }
    //检查B C D E 有无出现断层
    for (var j = 2; j < i; j++) {
        if (sheet["C" + j] == undefined && sheet["C" + (j + 1)] != undefined) {
            Alert1("提示", "C" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["E" + j] == undefined && sheet["E" + (j + 1)] != undefined) {
            Alert1("提示", "E" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["G" + j] == undefined && sheet["G" + (j + 1)] != undefined) {
            Alert1("提示", "G" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
        if (sheet["L" + j] == undefined && sheet["L" + (j + 1)] != undefined) {
            Alert1("提示", "L" + j + "没有填写数据,请检查数据完整性", "btn-danger", "red");
            return false;
        }
    }
    if (sheet["B" + i] == undefined || sheet["C" + i] == undefined || sheet["D" + i] == undefined || sheet["E" + i] == undefined) {
        Alert1("提示", "请检查第" + i + "行数据的完整性", "btn-danger", "red");
        return false;
    }
    return true;
}
