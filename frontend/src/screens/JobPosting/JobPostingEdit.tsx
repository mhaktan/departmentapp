import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type JobPostingRecord = {
  id: string | number;
  title: string;
  description?: string;
  requirements?: string;
  positionCount: number;
  status: string;
  publishDate?: string;
  closingDate?: string;
  employmentType: string;
  departmentId: string;
};

interface JobPostingEditProps {
  record: JobPostingRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const JobPostingEdit: React.FC<JobPostingEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<JobPostingRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<JobPostingRecord>) =>
      dataProvider.update('JobPosting', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'JobPosting', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit JobPosting</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Title *" value={String(form.title ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('title', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Description" value={String(form.description ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('description', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Requirements" value={String(form.requirements ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('requirements', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Position Count *" value={String(form.positionCount ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('positionCount', Number(v)))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Draft', value: '0' }, { label: 'Published', value: '1' }, { label: 'Closed', value: '2' }, { label: 'Cancelled', value: '3' }]} />
                </div>
                <div>
                  <TkDatepicker label="Publish Date" value={String(form.publishDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('publishDate', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Closing Date" value={String(form.closingDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('closingDate', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Employment Type *" value={String(form.employmentType ?? '')} onChange={(v) => setField('employmentType', v ? Number(v) : null)} searchable={false} options={[{ label: 'FullTime', value: '0' }, { label: 'PartTime', value: '1' }, { label: 'Contract', value: '2' }, { label: 'Intern', value: '3' }]} />
                </div>
                <div>
                  <LookupSelect label="Departman *" resource="Department" value={String(form.departmentId ?? '')} onChange={(v) => setField('departmentId', v)} />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Save Changes'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
