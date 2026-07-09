import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type OvertimeRecordRecord = {
  id: string | number;
  overtimeDate: string;
  hours: number;
  reason?: string;
  status: string;
  approverNote?: string;
  notes?: string;
  managerApproverId?: number;
  employeeId: string;
};

interface OvertimeRecordEditProps {
  record: OvertimeRecordRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const OvertimeRecordEdit: React.FC<OvertimeRecordEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<OvertimeRecordRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<OvertimeRecordRecord>) =>
      dataProvider.update('OvertimeRecord', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'OvertimeRecord', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit OvertimeRecord</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkDatepicker label="Overtime Date *" value={String(form.overtimeDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('overtimeDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Hours *" value={String(form.hours ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hours', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Reason" value={String(form.reason ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('reason', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Pending', value: '0' }, { label: 'Approved', value: '1' }, { label: 'Rejected', value: '2' }, { label: 'Cancelled', value: '3' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Approver Note" value={String(form.approverNote ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('approverNote', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Notes" value={String(form.notes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('notes', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Manager Approver Id" value={String(form.managerApproverId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('managerApproverId', Number(v)))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Personel *" resource="Employee" value={String(form.employeeId ?? '')} onChange={(v) => setField('employeeId', v)} displayField="employeeNumber" />
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
