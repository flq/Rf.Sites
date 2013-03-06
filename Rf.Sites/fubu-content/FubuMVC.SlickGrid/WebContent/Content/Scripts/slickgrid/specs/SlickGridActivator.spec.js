describe('slickGridColumns', function () {
    it('can tell me the initial columns without any customization and all columns displayed', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3'}];

        var columns = Slick.GridColumns(data);

        var initialData = columns.getDisplayedColumns();

        expect(initialData.length).toEqual(3);
        expect(initialData[0].id).toEqual('f1');
        expect(initialData[1].id).toEqual('f2');
        expect(initialData[2].id).toEqual('f3');
    });

    it('can tell me the initial columns without any customization and some columns are initially hidden', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2', displayed: false }, { name: 'f3-name', id: 'f3'}];

        var columns = Slick.GridColumns(data);

        var initialData = columns.getDisplayedColumns();

        expect(initialData.length).toEqual(2);
        expect(initialData[0].id).toEqual('f1');
        expect(initialData[1].id).toEqual('f3');
    });

    it('can tell me the initial columns with customization and some columns are initially hidden', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3'}];

        var columns = Slick.GridColumns(data);
        var customizations = {
            columns: {
                'f1': { width: 100 },
                'f2': { displayed: false }
            }
        };

        columns.applyCustomizations(customizations);
        var initialData = columns.getDisplayedColumns();

        expect(initialData.length).toEqual(2);
        expect(initialData[0].id).toEqual('f1');
        expect(initialData[1].id).toEqual('f3');

        expect(initialData[0].width).toEqual(100);
    });

    it('can configure a new arrangement of columns', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3'}];

        var columns = Slick.GridColumns(data);

        var newColumns = columns.displayColumns(['f3', 'f1']);

        expect(newColumns.length).toEqual(2);
        expect(newColumns[0].id).toEqual('f3');
        expect(newColumns[1].id).toEqual('f1');
    });

    it('can get all columns', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3' }, { name: 'f4-name', id: 'f4'}];

        var columns = Slick.GridColumns(data);

        var grid = {
            getColumns: function () {
                return data;
            }
        };

        var columnData1 = columns.getAllColumns(grid);
        expect(columnData1.displayed.length).toEqual(4);
        expect(columnData1.displayed[0].id).toEqual('f1');
        expect(columnData1.displayed[0].name).toEqual('f1-name');

        expect(columnData1.displayed[0].id).toEqual('f1');
        expect(columnData1.displayed[1].id).toEqual('f2');
        expect(columnData1.displayed[2].id).toEqual('f3');

        expect(columnData1.hidden.length).toEqual(0);

        grid.getColumns = function () {
            return [{ name: 'f3-name', id: 'f3' }, { name: 'f1-name', id: 'f1'}];
        }

        columns.displayColumns(['f3', 'f1']);
        var columnData2 = columns.getAllColumns(grid);

        expect(columnData2.displayed.length).toEqual(2);
        expect(columnData2.displayed[0].id).toEqual('f3');
        expect(columnData2.displayed[1].id).toEqual('f1');

        expect(columnData2.hidden.length).toEqual(2);
        expect(columnData2.hidden[0].id).toEqual('f2');
        expect(columnData2.hidden[0].name).toEqual('f2-name');
    });

    it('can tell me the freeze index with no frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(-1);
    });

    it('can tell me no frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1' }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(0);
    });

    it('can tell me the freeze index with frozen hidden columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true, displayed: false }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(-1);
    });

    it('can tell me the frozen columns with frozen hidden columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true, displayed: false }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(0);
    });

    it('can tell me the freeze index with consecutive frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2', frozen: true }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(1);
    });

    it('can tell me the frozen columns with consecutive frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2', frozen: true }, { name: 'f3-name', id: 'f3' }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(2);
        expect(actual[0].id).toEqual('f1');
        expect(actual[1].id).toEqual('f2');
    });

    it('can tell me the freeze index with nonconsecutive frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(0);
    });

    it('can tell me the frozen columns with nonconsecutive frozen columns', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2' }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(1);
        expect(actual[0].id).toEqual('f1');
    });

    it('can tell me the freeze index with consecutive frozen columns and hidden first column', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true, displayed: false }, { name: 'f2-name', id: 'f2', frozen: true }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(1);
    });

    it('can tell me the frozen columns with consecutive frozen columns and hidden first column', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true, displayed: false }, { name: 'f2-name', id: 'f2', frozen: true }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(2);
        expect(actual[0].id).toEqual('f2');
        expect(actual[1].id).toEqual('f3');
    });

    it('can tell me the freeze index with consecutive frozen columns and hidden second column', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2', frozen: true, displayed: false }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var index = columns.getFrozenColumnIndex();

        expect(index).toEqual(1);
    });

    it('can tell me the frozen columns with consecutive frozen columns and hidden second column', function () {
        var data = [{ name: 'f1-name', id: 'f1', frozen: true }, { name: 'f2-name', id: 'f2', frozen: true, displayed: false }, { name: 'f3-name', id: 'f3', frozen: true }];
        var columns = Slick.GridColumns(data);

        var actual = columns.getFrozenColumns();

        expect(actual.length).toEqual(2);
        expect(actual[0].id).toEqual('f1');
        expect(actual[1].id).toEqual('f3');
    });
});