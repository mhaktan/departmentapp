import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { TrainingParticipationCreate } from './TrainingParticipationCreate';
import { TrainingParticipationEdit } from './TrainingParticipationEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type TrainingParticipationRecord = {
  id: string | number;
  attended: boolean;
  completionDate?: string;
  score?: number;
  certificateEarned?: boolean;
  notes?: string;
  trainingId: string;
  employeeId: string;
  [key: string]: unknown;
};

// ---------------------------------------------------------------------------
// Column definition — edit this array to add/remove/reorder columns
// ---------------------------------------------------------------------------
//
// Override examples:
//   • Hide a column:     remove its entry from COLUMNS
//   • Add a custom col:  { field: 'fullName', header: 'Full Name', html: (row) => `${row.firstName} ${row.lastName}` }
//   • Enable filtering:  add searchable: true  or  filterType: 'text' | 'checkbox' | 'radio' | 'datepicker'
//   • Custom cell render: html: (row) => `<span style="color:green">${row.status}</span>`
//
// Shared components (src/shared/) can be edited to change behavior globally:
//   • ListPageLayout  — table wrapper, pagination, header layout
//   • ActionButtons   — edit/delete button styles, labels, and behavior
//   • useListQuery    — data fetching, sorting, filtering logic
//   • useDeleteMutation / DeleteConfirmDialog — delete flow
//
// Action buttons override: edit src/shared/ActionButtons.ts DEFAULT_CONFIG
// or pass custom config: actionColumn('id', { hasEdit: true, config: { edit: { label: 'View', style: '...' } } })
//

const COLUMNS: TableColumn[] = [
  { field: 'id', header: 'ID', sortable: true },
  { field: 'attended', header: 'Attended', sortable: true, filterType: 'checkbox', filterOptions: [{ label: 'Yes', value: 'true' }, { label: 'No', value: 'false' }], html: (row: Record<string, unknown>) => row.attended ? '<span style="color:#2e7d32;font-weight:600">Yes</span>' : '<span style="color:#999">No</span>' },
  { field: 'completionDate', header: 'Completion Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.completionDate ? new Date(String(row.completionDate)).toLocaleDateString() : '—' },
  { field: 'score', header: 'Score', sortable: true },
  { field: 'certificateEarned', header: 'Certificate Earned', sortable: true, filterType: 'checkbox', filterOptions: [{ label: 'Yes', value: 'true' }, { label: 'No', value: 'false' }], html: (row: Record<string, unknown>) => row.certificateEarned ? '<span style="color:#2e7d32;font-weight:600">Yes</span>' : '<span style="color:#999">No</span>' },
  { field: 'notes', header: 'Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'trainingId', header: 'Eğitim', sortable: true },
  { field: 'employeeId', header: 'Personel', sortable: true },
];

// ---------------------------------------------------------------------------
// TrainingParticipationList
// ---------------------------------------------------------------------------

export const TrainingParticipationList: React.FC = () => {
  const list = useListQuery<TrainingParticipationRecord>({ resource: 'TrainingParticipation' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<TrainingParticipationRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<TrainingParticipationRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('TrainingParticipation', (ids) => { ids.forEach(id => triggerFlows('delete', 'TrainingParticipation', { id })); });


  const columns = [...COLUMNS, ...actionColumn('id', { hasEdit: true, hasDelete: true })];

  const handleCrudAction = useCallback((action: string, id: string) => {
    const row = list.records.find((r) => String(r.id) === id);
    if (!row) return;
    if (action === 'edit') setEditRecord(row);
    if (action === 'delete') del.requestSingleDelete(row.id);
  }, [list.records]);

  return (
    <>
      <ListPageLayout
        title="TrainingParticipation"
        subtitle={Object.keys(list.displayParams).length > 0 ? (
          <div style={{ fontSize: 13, color: '#666', marginTop: 4 }}>
            {Object.entries(list.displayParams).map(([k, v]) => (
              <span key={k} style={{ marginRight: 12 }}>{k}: <strong>{v}</strong></span>
            ))}
          </div>
        ) : undefined}
        records={list.records}
        columns={columns}
        dataKey="id"
        total={list.total}
        loading={list.isLoading}
        page={list.page}
        perPage={list.perPage}
        onPageChange={list.setPage}
        onPerPageChange={list.setPerPage}
        onTableRequest={list.handleTableRequest}
        selectionMode="checkbox"
        selectedRows={selectedRows}
        onSelectionChange={(rows) => setSelectedRows(rows as TrainingParticipationRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create TrainingParticipation" variant="primary" onTkClick={() => setShowCreate(true)} />
        </>}
      />

      {list.isError && (
        <div style={{ padding: '10px 14px', background: '#fff3f3', border: '1px solid #f5c6c6', borderRadius: 6, color: '#c62828', fontSize: 13, marginBottom: 12 }}>
          Failed to load data: {(list.error as Error).message}
        </div>
      )}

      <DeleteConfirmDialog
        visible={!!del.deleteTarget}
        label={del.deleteTarget?.label ?? ''}
        isPending={del.isPending}
        onConfirm={del.confirmDelete}
        onCancel={() => del.setDeleteTarget(null)}
      />
      <TrainingParticipationCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <TrainingParticipationEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

