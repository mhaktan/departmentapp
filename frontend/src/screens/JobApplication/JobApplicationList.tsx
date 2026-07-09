import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { JobApplicationCreate } from './JobApplicationCreate';
import { JobApplicationEdit } from './JobApplicationEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type JobApplicationRecord = {
  id: string | number;
  applicantFirstName: string;
  applicantLastName: string;
  applicantEmail: string;
  applicantPhone?: string;
  coverLetter?: string;
  status: string;
  screeningNotes?: string;
  interviewDate?: string;
  interviewNotes?: string;
  offerSalary?: number;
  offerDate?: string;
  rejectionReason?: string;
  jobPostingId: string;
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
  { field: 'applicantFirstName', header: 'Applicant First Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'applicantLastName', header: 'Applicant Last Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'applicantEmail', header: 'Applicant Email', sortable: true, searchable: true, filterType: 'text' },
  { field: 'applicantPhone', header: 'Applicant Phone', sortable: true, searchable: true, filterType: 'text' },
  { field: 'coverLetter', header: 'Cover Letter', sortable: true, searchable: true, filterType: 'text' },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Received', value: '0' }, { label: 'Screening', value: '1' }, { label: 'Interview', value: '2' }, { label: 'OfferPending', value: '3' }, { label: 'OfferAccepted', value: '4' }, { label: 'OfferRejected', value: '5' }, { label: 'Rejected', value: '6' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Received', '1': 'Screening', '2': 'Interview', '3': 'OfferPending', '4': 'OfferAccepted', '5': 'OfferRejected', '6': 'Rejected'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'screeningNotes', header: 'Screening Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'interviewDate', header: 'Interview Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.interviewDate ? new Date(String(row.interviewDate)).toLocaleDateString() : '—' },
  { field: 'interviewNotes', header: 'Interview Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'offerSalary', header: 'Offer Salary', sortable: true },
  { field: 'offerDate', header: 'Offer Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.offerDate ? new Date(String(row.offerDate)).toLocaleDateString() : '—' },
  { field: 'rejectionReason', header: 'Rejection Reason', sortable: true, searchable: true, filterType: 'text' },
  { field: 'jobPostingId', header: 'İş İlanı', sortable: true },
];

// ---------------------------------------------------------------------------
// JobApplicationList
// ---------------------------------------------------------------------------

export const JobApplicationList: React.FC = () => {
  const list = useListQuery<JobApplicationRecord>({ resource: 'JobApplication' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<JobApplicationRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<JobApplicationRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('JobApplication', (ids) => { ids.forEach(id => triggerFlows('delete', 'JobApplication', { id })); });


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
        title="JobApplication"
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
        onSelectionChange={(rows) => setSelectedRows(rows as JobApplicationRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create JobApplication" variant="primary" onTkClick={() => setShowCreate(true)} />
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
      <JobApplicationCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <JobApplicationEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

